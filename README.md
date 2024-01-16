PROCEDURE GETALLERRORREQUESTS(FROMDATE NVARCHAR2,
                             TODATE   NVARCHAR2,
                             CUR_OUT  OUT SYS_REFCURSOR) IS
  BEGIN
  
    OPEN CUR_OUT FOR
    
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
                        (SELECT ES.ERROR_TEXT вот тут я хотел бы, в рамках одного подслелекта получить 2 столбца, но oracle ругается что сллишком много значений.
                        FROM CARDCOLV.TCR_EXECUTION_STEPS ES
                        WHERE ES.ID = (SELECT MAX(ES.ID)
                                FROM CARDCOLV.TCR_EXECUTION_STEPS ES
                                WHERE ES.REQUEST_NUMBER = R.REQUEST_NUMBER
                                 AND ES.STEP <> 'ENDTRANSACTION'
                                 AND ES.STEP <> 'LOGOFF')),
                        (SELECT T.D_NAME
                FROM CARDCOLV.TCR_CIA_DEVICES T 
               WHERE T.ID = RHD.TCRID) AS TCRMODEL
        FROM CARDCOLV.TCR_REQUESTS R, CARDCOLV.TCR_REQUEST_HISTORY_DATA RHD
       WHERE R.UPDATE_DATE > TO_DATE(FROMDATE, 'dd.mm.yyyy hh24:mi:ss')
         AND R.UPDATE_DATE < TO_DATE(TODATE, 'dd.mm.yyyy hh24:mi:ss')
         AND R.STATUS = 'Error'
         AND R.REQUEST_NUMBER = RHD.REQUEST_NUMBER
         AND R.LOGTYPE NOT IN ('197','200', '205')
         AND R.STATUS = RHD.STATUS;

END GETALLERRORREQUESTS;
