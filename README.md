create or replace function get_test_case_data(in_scheme_name text,
                        test_case_number int)
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
