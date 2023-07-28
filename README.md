CREATE OR REPLACE PROCEDURE public.add_new_field(IN in_scheme_name character varying, IN in_field_name character varying, OUT out_result character varying)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
    INSERT INTO  AUTOTEST_FIELD
      (ID, PROCESS_ID, FIELD_NAME)
    VALUES
      ((SELECT MAX(f2.ID) + 1 FROM  AUTOTEST_FIELD f2),
       (SELECT P.ID
          FROM  AUTOTEST_PROCESS P
         WHERE P.SCHEME_NAME = IN_SCHEME_NAME),
       IN_FIELD_NAME);
 
  
    OUT_RESULT := 'Ok';
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.add_process(IN in_scheme_name character varying, IN in_process_name character varying, OUT out_result character varying)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
    INSERT INTO AUTOTEST_PROCESS
      (ID, SCHEME_NAME, PROCESS_NAME)
    VALUES
      ((SELECT MAX(P2.ID) + 1 FROM  AUTOTEST_PROCESS P2),
       IN_SCHEME_NAME,
       IN_PROCESS_NAME);
  
  
    OUT_RESULT := 'Ok';
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;
  $procedure$
;

CREATE OR REPLACE PROCEDURE public.add_test_case(IN in_scheme_name character varying, IN case_number numeric, IN description character varying, OUT out_result character varying)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
    INSERT INTO  AUTOTEST_TEST_CASE
      (ID, PROCESS_ID, CASE_NUMBER, DESCRIPTION)
    VALUES
      ((SELECT MAX(TC2.ID) + 1 FROM  AUTOTEST_TEST_CASE TC2),
       (SELECT P.ID
          FROM  AUTOTEST_PROCESS P
         WHERE P.SCHEME_NAME = IN_SCHEME_NAME),
       CASE_NUMBER,
       DESCRIPTION);
    
  
    OUT_RESULT := 'Ok';
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ; $procedure$
;

CREATE OR REPLACE PROCEDURE public.get_fields_list(IN in_scheme_name character varying, INOUT out_fields_list refcursor)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
    OPEN OUT_FIELDS_LIST for
      SELECT F.ID, F.PROCESS_ID, F.FIELD_NAME
        FROM AUTOTEST_FIELD F
       WHERE F.PROCESS_ID =
             (SELECT P.ID
              FROM AUTOTEST_PROCESS P
               WHERE P.SCHEME_NAME = IN_SCHEME_NAME)
       ORDER BY F.FIELD_NAME;
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.get_processes_list(OUT out_processes_list refcursor)
 LANGUAGE plpgsql
AS $procedure$ BEGIN 
	 open OUT_PROCESSES_LIST for
	  SELECT ID, SCHEME_NAME, PROCESS_NAME
        FROM AUTOTEST_PROCESS
       ORDER BY SCHEME_NAME;
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.get_test_case_data(IN in_process_name character varying, IN in_test_case_number numeric, OUT out_fields_list refcursor)
 LANGUAGE plpgsql
AS $procedure$ 
   declare TEST_CASE_ID_ numeric;
   BEGIN
    SELECT TC.ID
      INTO TEST_CASE_ID_
      FROM  AUTOTEST_PROCESS P,  AUTOTEST_TEST_CASE TC
     WHERE P.SCHEME_NAME = IN_PROCESS_NAME
       AND TC.PROCESS_ID = P.ID
       AND TC.CASE_NUMBER = IN_TEST_CASE_NUMBER;
  
       SELECT FV.TEST_CASE_ID || '_' || FV.FIELD_ID COMBO_ID,
                     FV.VALUE
                FROM  AUTOTEST_FIELD_VALUE FV
               WHERE FV.TEST_CASE_ID = TEST_CASE_ID_;
    OPEN OUT_FIELDS_LIST FOR
      SELECT NAME, VALUE
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
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.get_test_cases_list(IN in_process_name character varying, OUT out_test_cases_list refcursor)
 LANGUAGE plpgsql
AS $procedure$ BEGIN
    OPEN OUT_TEST_CASES_LIST FOR
      SELECT TC.ID, TC.PROCESS_ID, TC.CASE_NUMBER, TC.DESCRIPTION
        FROM  AUTOTEST_PROCESS P,  AUTOTEST_TEST_CASE TC
       WHERE P.SCHEME_NAME = IN_PROCESS_NAME
         AND TC.PROCESS_ID = P.ID;
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.update_test_case_description(IN in_process_name character varying, IN in_test_case_number numeric, IN in_field_description character varying, OUT out_result character varying)
 LANGUAGE plpgsql
AS $procedure$ declare  TEST_CASE_COUNT numeric; 
   BEGIN
    SELECT COUNT(TC.ID)
      INTO TEST_CASE_COUNT
      FROM  AUTOTEST_PROCESS P,  AUTOTEST_TEST_CASE TC
     WHERE P.SCHEME_NAME = IN_PROCESS_NAME
       AND TC.PROCESS_ID = P.ID
       AND TC.CASE_NUMBER = IN_TEST_CASE_NUMBER;
  
    IF TEST_CASE_COUNT = 1 THEN
      UPDATE  AUTOTEST_TEST_CASE TC
         SET TC.DESCRIPTION = IN_FIELD_DESCRIPTION
       WHERE TC.PROCESS_ID =
             (SELECT P.ID
                FROM  AUTOTEST_PROCESS P
               WHERE P.SCHEME_NAME = IN_PROCESS_NAME)
         AND TC.CASE_NUMBER = IN_TEST_CASE_NUMBER;
      COMMIT;
      OUT_RESULT := 'Ok';
    ELSE
      OUT_RESULT := 'UpdateError: found (' || TEST_CASE_COUNT || ') rows';
    END IF;
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;
 $procedure$
;

CREATE OR REPLACE PROCEDURE public.update_test_case_field_value(IN in_process_name character varying, IN in_test_case_number numeric, IN in_field_name character varying, IN in_field_value character varying, OUT out_result character varying)
 LANGUAGE plpgsql
AS $procedure$
  declare 
  FIELD_ID_     numeric;
    TEST_CASE_ID_ numeric;
    CNT           numeric;
  BEGIN
    SELECT TC.ID
      INTO TEST_CASE_ID_
      FROM  AUTOTEST_PROCESS P,  AUTOTEST_TEST_CASE TC
     WHERE P.SCHEME_NAME = IN_PROCESS_NAME
       AND TC.PROCESS_ID = P.ID
       AND TC.CASE_NUMBER = IN_TEST_CASE_NUMBER;
  
    SELECT F.ID
      INTO FIELD_ID_
      FROM  AUTOTEST_TEST_CASE TC,  AUTOTEST_FIELD F
     WHERE TC.ID = TEST_CASE_ID_
       AND TC.PROCESS_ID = F.PROCESS_ID
       AND F.FIELD_NAME = IN_FIELD_NAME;
  
    SELECT COUNT(1)
      INTO CNT
      FROM  AUTOTEST_FIELD_VALUE FV
     WHERE FV.TEST_CASE_ID = TEST_CASE_ID_
       AND FV.FIELD_ID = FIELD_ID_;
  
    IF CNT = 0 THEN
     if  IN_FIELD_VALUE IS NOT NULL THEN
        INSERT INTO  AUTOTEST_FIELD_VALUE 
          (TEST_CASE_ID, FIELD_ID, VALUE)
        VALUES
          (TEST_CASE_ID_, FIELD_ID_, IN_FIELD_VALUE);
        COMMIT;
        --ELSE DO NOTHING. We don't need empty fields to be added
      END if ;
    ELSE
      UPDATE  AUTOTEST_FIELD_VALUE FV
         SET FV.VALUE = IN_FIELD_VALUE
       WHERE FV.TEST_CASE_ID = TEST_CASE_ID_
         AND FV.FIELD_ID = FIELD_ID_;
      COMMIT;
    END IF;
  
    OUT_RESULT := 'Ok';
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;$procedure$
;
