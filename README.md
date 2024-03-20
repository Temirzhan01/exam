SELECT "a"."IDX", "a"."ATTORNEY_DATE", "a"."ATTORNEY_NUMBER", "a"."BRANCH_CODE", "a"."BRANCH_NAME", "a"."BRANCH_NAME_QAZ", "a"."DOMAIN", "a"."FULL_NAME", "a"."LOGIN", "a"."POSITION", "a"."POSITION_QAZ", "a"."VALIDITY"
FROM "cardcolv"."AUTHORITIES_MSB" "a"
WHERE (((((("a"."BRANCH_CODE" LIKE '%DRKK%') OR (("a"."BRANCH_CODE" = N'DKA'))))) OR 
((((("a"."BRANCH_CODE" = N'ZPP')) AND ((TRUNC(SYSDATE) < (("a"."VALIDITY" + NUMTODSINTERVAL(1.0E0,'DAY'))))))))))

Смотри я получил такой запрос, если его попробовать в sql developer запустить, то ругается что "ORA-00942: таблица или представление пользователя не существует" 
Но, если из "cardcolv"."AUTHORITIES_MSB" "a"  и я уберу все " и оставлю cardcolv.AUTHORITIES_MSB то отрабатывает
