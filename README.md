create or replace procedure add_test_case(in_scheme_name text,
					test_case_number int,
					description text)
as $$
begin 
	insert into autotest_test_case (process_id, case_number, description) values ((select id from autotest_process where scheme_name = in_scheme_name), test_case_number, description);
end; $$
language plpgsql



create or replace function get_test_case(in_scheme_name text)
returns SETOF autotest_test_case as $$
begin 
	return query select * from autotest_test_case where autotest_test_case.process_id = (select id from autotest_process where scheme_name = in_scheme_name);
end; $$
language plpgsql



create or replace procedure update_test_case_description(in_scheme_name text, 
							test_case_number int,
							in_description text)
as $$
declare  test_case_count integer; 
begin 
	    select count(autotest_test_case.id) into test_case_count from  autotest_process,  autotest_test_case
		where autotest_process.scheme_name = in_scheme_name 
		and autotest_test_case.process_id = autotest_process.id 
		and autotest_test_case.case_number = test_case_number;
		
	if test_case_count = 1 then
		update  autotest_test_case
			set description = in_description
			where autotest_test_case.process_id =
				(select autotest_process.id from  autotest_process
				 where autotest_process.scheme_name = in_scheme_name)
			and autotest_test_case.case_number = test_case_number;
	end if;
				 
end $$
language plpgsql



create or replace function get_field_list(in_scheme_name text)
returns SETOF autotest_field as $$
begin 
	return query select * from autotest_field where autotest_field.process_id = (select id from autotest_process where scheme_name = in_scheme_name);
end;  $$
language plpgsql



create or replace procedure add_field(in_scheme_name text,
					field text)
as $$
begin 
	insert into autotest_field (process_id, field_name) values ((select id from autotest_process where scheme_name = in_scheme_name), field);
end; $$
language plpgsql



create or replace function get_test_case_data(in_scheme_name text,
						test_case_number int)
returns table (field_name text, field_value text) 
as $$
declare case_id int;
begin
	select tc.id into case_id from autotest_test_case tc, autotest_process p
	where p.scheme_name = in_scheme_name and tc.process_id = p.id and tc.case_number = test_case_number;
	
	return query SELECT af.field_name, fv.field_value 
	FROM autotest_process ap JOIN autotest_test_case tc ON ap.id = tc.process_id 
	JOIN autotest_field af ON ap.id = af.process_id 
	JOIN autotest_field_value fv ON tc.id = fv.test_case_id 
	AND af.id = fv.field_id 
	WHERE ap.scheme_name = in_scheme_name
	AND tc.id = case_id;
	
end; $$
language plpgsql

