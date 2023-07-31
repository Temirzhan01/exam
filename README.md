create or replace procedure delete_test_case(in_scheme_name text, test_case_number int)
as $$
begin
	delete from autotest_test_case tc where tc.process_id = (select p.id from autotest_process p where p.scheme_name = in_scheme_name) and tc.case_number = case_number;
end; $$
language plpgsql
