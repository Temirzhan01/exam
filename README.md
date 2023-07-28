UPDATE table4 
SET value = 'новое значение'
FROM table1 t1
JOIN table2 t2 ON t1.id = t2.proccess_id
JOIN table3 t3 ON t1.id = t3.proccess_id
WHERE t2.id = table4.case_id AND t3.id = table4.field_id AND t1.name = 'Имя из таблицы 1' AND t2.number = 'число из таблицы 2';
