CREATE OR REPLACE PROCEDURE update_test_case_field_value(IN in_process_name text, IN in_test_case_number int, IN in_field_name text, IN in_field_value text, OUT out_result text)
 LANGUAGE plpgsql
AS $procedure$
  declare 
  FIELD_ID_     int;
    TEST_CASE_ID_ int;
    CNT           int;
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
          (TEST_CASE_ID, FIELD_ID, FIELD_VALUE)
        VALUES
          (TEST_CASE_ID_, FIELD_ID_, IN_FIELD_VALUE);
        COMMIT;
        --ELSE DO NOTHING. We don't need empty fields to be added
      END if ;
    ELSE
      UPDATE  AUTOTEST_FIELD_VALUE
         SET FIELD_VALUE = IN_FIELD_VALUE
       WHERE TEST_CASE_ID = TEST_CASE_ID_
         AND FIELD_ID = FIELD_ID_;
      COMMIT;
    END IF;
  
    OUT_RESULT := 'Ok';
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;$procedure$
;
