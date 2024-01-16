SELECT DISTINCT R.REQUEST_NUMBER,
                    R.UPDATE_DATE,
                      CASE
WHEN R.LOGTYPE = '195' THEN
'Выдача наличных ЮЛ'
WHEN R.LOGTYPE = '196' THEN
'Прием наличных ЮЛ'
WHEN R.LOGTYPE = '198' THEN
'Выдача наличных OnlineBank'
WHEN R.LOGTYPE = '199' THEN
'Прием наличных OnlineBank'
WHEN R.LOGTYPE = '201' THEN
'Выдача наличных ФЛ'
WHEN R.LOGTYPE = '202' THEN
'Прием наличных ФЛ'
WHEN R.LOGTYPE = '203' THEN
'Выдача наличных ФЛ HomeBank'
WHEN R.LOGTYPE = '204' THEN
'Прием наличных ФЛ HomeBank'
                        ELSE
                         ''
                      END OPERATION,
                      RHD.CURRENCY,
                      R.INITIATOR,
                      (SELECT U.SURNAME || ' ' || U.NAME
                         FROM CARDCOLV.T_USERS U
                        WHERE U.LOGIN = R.INITIATOR) AS UNAME,
                        ES.ERROR_TEXT,
                        ES.ERROR_DESCRIPTION, -- тут мы ожидаем clob как исправить ранее отправленную ошибку?
                        (SELECT T.D_NAME
                FROM CARDCOLV.TCR_CIA_DEVICES T 
               WHERE T.ID = RHD.TCRID) AS TCRMODEL
        FROM CARDCOLV.TCR_REQUESTS R, CARDCOLV.TCR_REQUEST_HISTORY_DATA RHD, CARDCOLV.TCR_EXECUTION_STEPS ES
       WHERE R.UPDATE_DATE > TO_DATE('15.01.2024 15:00:00', 'dd.mm.yyyy hh24:mi:ss')
         AND R.UPDATE_DATE < TO_DATE('15.01.2024 17:00:00', 'dd.mm.yyyy hh24:mi:ss')
         AND R.STATUS = 'Error'
         AND R.REQUEST_NUMBER = RHD.REQUEST_NUMBER
         AND R.LOGTYPE NOT IN ('197','200', '205')
         AND R.STATUS = RHD.STATUS
         AND ES.ID = (SELECT MAX(ES.ID)
             FROM CARDCOLV.TCR_EXECUTION_STEPS ES
             WHERE ES.REQUEST_NUMBER = R.REQUEST_NUMBER
             AND ES.STEP <> 'ENDTRANSACTION'
             AND ES.STEP <> 'LOGOFF');
