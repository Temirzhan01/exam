CREATE OR REPLACE PROCEDURE add_process(IN in_scheme_name text, IN in_process_name text, OUT out_result text)
 LANGUAGE plpgsql
AS $$ BEGIN
    INSERT INTO AUTOTEST_PROCESS
      (SCHEME_NAME, PROCESS_NAME)
    VALUES
       (IN_SCHEME_NAME,
       IN_PROCESS_NAME);
  
  
    OUT_RESULT := 'Ok';
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ;
  $$
;
