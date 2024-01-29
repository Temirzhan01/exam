Смотри, у нас есть старый сервис на .net framework 4.5. Там мы обращаемся к бд oracle, и получаем данные
Задача какая, сейчас переписываю сервис на asp.net core 6.0. Там используем Oracle entityframework. 
В бд есть таблица, для которой я создал модель, для этой сущности есть dbset из этой модели. 
Теперь, задача в чем. Я пишу на 3 слойной архитектуре. Контроллер, сервис, репозиторий.
Вот, тут есть условие выборки, как бы написать в целом? Как в целом обычно пишется в репозиторий? 
                
                if (branch_code == "DRKK1" || branch_code == "DRKK2" || branch_code == "DRKK3")
                {
                    cmd.CommandText = "select * from cardcolv.AUTHORITIES_MSB WHERE branch_code like '%" + branch_code.Substring(0, 4) + "%' OR branch_code = 'DKA' OR branch_code = 'ZPP' AND trunc(sysdate) < validity + 1";
                }
                else
                {
                    cmd.CommandText = "select * from cardcolv.AUTHORITIES_MSB WHERE branch_code = '" + branch_code + "' AND trunc(sysdate) < validity + 1";
                }
