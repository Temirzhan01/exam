DECLARE
  -- Объявляем переменную курсора
  cur SYS_REFCURSOR;
  
  -- Определяем тип записи, соответствующий структуре результата
  TYPE myType IS RECORD(
     CHOOSE_PRC VARCHAR2(100) -- Предполагается, что возвращается одно поле
  );
  
  -- Создаем переменную этого типа для хранения извлеченных данных
  myRecord myType;

BEGIN
  -- Включаем буферизацию вывода для отображения результатов
  dbms_output.enable;
  
  -- Вызываем удаленную процедуру через db_link, предполагая, что она правильно открывает курсор
  kzc_bwx.opt_spm.CHOOSE_PRC_FOR_IBFL@PROD.KZC('010904500574', cur);
  
  -- Проверяем, был ли курсор успешно открыт и содержит ли он строки
  IF cur IS NOT NULL THEN
    LOOP
      FETCH cur INTO myRecord;
      EXIT WHEN cur%NOTFOUND;
      
      -- Выводим значение поля CHOOSE_PRC
      dbms_output.put_line('Result: ' || myRecord.CHOOSE_PRC);
    END LOOP;
    
    -- Закрываем курсор после завершения работы с ним
    CLOSE cur;
  ELSE
    dbms_output.put_line('Cursor is not open.');
  END IF;
  
EXCEPTION
  -- Обрабатываем исключения, которые могут возникнуть при работе с курсором
  WHEN NO_DATA_FOUND THEN
    dbms_output.put_line('No data found.');
  WHEN TOO_MANY_ROWS THEN
    dbms_output.put_line('More than one row returned.');
  WHEN OTHERS THEN
    dbms_output.put_line('An unexpected error occurred: ' || SQLERRM);
    -- Попытка закрыть курсор, если он все еще открыт
    IF cur%ISOPEN THEN
      CLOSE cur;
    END IF;
END;
