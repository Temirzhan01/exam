CREATE OR REPLACE FUNCTION get_processes_list()
    RETURNS SETOF autotest_process
    LANGUAGE 'plpgsql'
AS $$
begin 
	return query select * from autotest_process;
end;  $$;



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



CREATE OR REPLACE FUNCTION get_field_list(in_scheme_name text)
    RETURNS SETOF autotest_field 
    LANGUAGE 'plpgsql'
AS $$
begin 
	return query select * from autotest_field where autotest_field.process_id = (select id from autotest_process where scheme_name = in_scheme_name);
end; $$;



CREATE OR REPLACE PROCEDURE add_field(IN in_scheme_name text, IN field text, OUT out_result text)
LANGUAGE 'plpgsql'
AS $$
begin 
	insert into autotest_field (process_id, field_name) 
	values ((select id from autotest_process where scheme_name = in_scheme_name), field);
	OUT_RESULT := 'Ok';
	
	EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
end; $$;



CREATE OR REPLACE PROCEDURE public.delete_field(
	IN in_scheme_name text,
	IN in_field_name text,
	OUT out_result text)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE fieldId int;
begin
	
	SELECT id INTO fieldId FROM autotest_field
   	where id = (
	   select id from autotest_field  
	   where process_id=(
		   select p.id from  autotest_process p
		   where p.scheme_name = in_scheme_name) 
	   and field_name = in_field_name);
	
	delete from autotest_field_value fv
	where fv.field_id = fieldId;

   	delete from autotest_field
   	where id = fieldId;
	   OUT_RESULT := 'Ok';
	
	EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
end; 
$BODY$;



CREATE OR REPLACE FUNCTION get_test_case(in_scheme_name text)
    RETURNS SETOF autotest_test_case 
    LANGUAGE 'plpgsql'
AS $$
begin 
	return query select * from autotest_test_case where autotest_test_case.process_id = (select id from autotest_process where scheme_name = in_scheme_name);
end; $$;



CREATE OR REPLACE PROCEDURE add_test_case(IN in_scheme_name text, IN test_case_number integer, IN description text, OUT out_result text)
LANGUAGE 'plpgsql'
AS $$
begin 
	insert into autotest_test_case (process_id, case_number, description) 
	values ((
		select id from autotest_process 
		where scheme_name = in_scheme_name), test_case_number, description);
		OUT_RESULT := 'Ok';
	
	EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
end; $$;



CREATE OR REPLACE PROCEDURE update_test_case_description(IN in_scheme_name text, IN test_case_number integer, IN in_description text, OUT out_result text)
LANGUAGE 'plpgsql'
AS $$
declare  test_case_count integer; 
begin 
	    select count(autotest_test_case.id) into test_case_count from  autotest_process,  autotest_test_case
		where autotest_process.scheme_name = in_scheme_name 
		and autotest_test_case.process_id = autotest_process.id 
		and autotest_test_case.case_number = test_case_number;
		
	IF test_case_count = 1 then
		update  autotest_test_case
			set description = in_description
			where autotest_test_case.process_id =
				(select autotest_process.id from  autotest_process
				 where autotest_process.scheme_name = in_scheme_name)
			and autotest_test_case.case_number = test_case_number;
      OUT_RESULT := 'Ok';
	ELSE
      OUT_RESULT := 'UpdateError: found (' || TEST_CASE_COUNT || ') rows';
    END IF;
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
				 
end $$;



CREATE OR REPLACE PROCEDURE delete_test_case(IN in_scheme_name text, IN test_case_number integer, OUT out_result text)
LANGUAGE 'plpgsql'
AS $$
begin
	delete from autotest_test_case tc 
	where tc.process_id = (
		select p.id from autotest_process p 
		where p.scheme_name = in_scheme_name) 
		and tc.case_number = test_case_number;
		OUT_RESULT := 'Ok';
	
	EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
end; $$;



create or replace function get_test_case_data(in_scheme_name text, test_case_number int) 
returns table (field_name text, field_value text) 
as $$ 
declare case_id int; 
begin 

select tc.id into case_id from autotest_test_case tc, autotest_process p 
where p.scheme_name = in_scheme_name and tc.process_id = p.id and tc.case_number = test_case_number;

return query SELECT af.field_name, fv.field_value 
FROM autotest_process ap 
JOIN autotest_test_case tc ON ap.id = tc.process_id 
LEFT JOIN autotest_field af ON ap.id = af.process_id 
LEFT JOIN autotest_field_value fv ON tc.id = fv.test_case_id 
AND af.id = fv.field_id 
WHERE ap.scheme_name = in_scheme_name
AND tc.id = case_id;

end; $$
language plpgsql



CREATE OR REPLACE PROCEDURE update_test_case_data(IN in_process_name text, IN in_test_case_number int, IN in_field_name text, IN in_field_value text, OUT out_result text)
 LANGUAGE plpgsql
AS $$
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
      END if ;
    ELSE
      UPDATE  AUTOTEST_FIELD_VALUE
         SET FIELD_VALUE = IN_FIELD_VALUE
       WHERE TEST_CASE_ID = TEST_CASE_ID_
         AND FIELD_ID = FIELD_ID_;
    END IF;
  
    OUT_RESULT := 'Ok';
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      OUT_RESULT := 'UpdateError: ' || SUBSTR(SQLERRM, 1, 200);
  END ; $$ ;
