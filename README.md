SELECT t3.name, t4.value
FROM table1 t1
JOIN table2 t2 ON t1.id = t2.proccess_id
JOIN table3 t3 ON t1.id = t3.proccess_id
JOIN table4 t4 ON t2.id = t4.case_id AND t3.id = t4.field_id
WHERE t1.name = 'Имя из таблицы 1' AND t2.number = 'число из таблицы 2';
