CREATE OR REPLACE FUNCTION public.get_test_case_data(
    IN in_process_name character varying, 
    IN in_test_case_number numeric
)
RETURNS TABLE(param text, param2 text)
LANGUAGE plpgsql
AS $function$
   declare TEST_CASE_ID_ numeric;
   BEGIN
    SELECT TC.ID
      INTO TEST_CASE_ID_
      FROM  AUTOTEST_PROCESS P,  AUTOTEST_TEST_CASE TC
     WHERE P.SCHEME_NAME = IN_PROCESS_NAME
       AND TC.PROCESS_ID = P.ID
       AND TC.CASE_NUMBER = IN_TEST_CASE_NUMBER;
       
    RETURN QUERY
      SELECT NAME as param, VALUE as param2
        FROM (SELECT TC.ID || '_' || F.ID COMBO_ID,
                     F.ID FIELD_ID,
                     F.FIELD_NAME NAME
                FROM  AUTOTEST_PROCESS   P,
                      AUTOTEST_TEST_CASE TC,
                      AUTOTEST_FIELD     F
               WHERE P.ID = TC.PROCESS_ID
                 AND P.ID = F.PROCESS_ID
                 AND TC.ID = TEST_CASE_ID_) TC right join 
             (SELECT FV.TEST_CASE_ID || '_' || FV.FIELD_ID COMBO_ID,
                     FV.VALUE
                FROM  AUTOTEST_FIELD_VALUE FV
               WHERE FV.TEST_CASE_ID = TEST_CASE_ID_ )FV 
                on TC.COMBO_ID = FV.COMBO_ID
       ORDER BY TC.FIELD_ID;
  END;
$function$;
