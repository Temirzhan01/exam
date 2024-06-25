<!DOCTYPE html>
<html lang="ru">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        span {
            font-family: 'Times New Roman';
        }

        .header p {
            margin-top: 0pt;
            margin-bottom: 0pt;
            line-height: 108%;
        }

        .content p {
            margin-top: 0pt;
            margin-bottom: 0pt;
            font-size: 8pt;
        }

        .inner p {
            text-align: center;
        }

        td {
            border-style: solid;
            border-width: 0.75pt;
            padding-right: 5.03pt;
            padding-left: 5.03pt;
        }
    </style>
</head>

<body>
    <div class="container" style="width:479.35pt;">
        <div class="header">
            <p style="text-align:center; font-size:10pt;"><strong><span>Кассалық кіріс ордері / Приходный кассовый ордер № 1 от {{date}} г.</span></strong></p>
            <p style="text-align:right; font-size:8pt;"><span>Уақыты / Время: {{time}}</span></p>
        </div>
        <table class="content" cellspacing="0" cellpadding="0" style="width:479.35pt; border-collapse:collapse;">
            <tbody>
                <tr style="height:32.4pt;">
                    <td colspan="5" style="width:249.75pt; vertical-align:middle;">
                        <p><span>{{nameKAZ}}</span></p>
                        <p><span>{{nameRUS}}</span></p>
                        <p><span>БСН / БИН {{bin}}</span></p>
                        <p><span>ББК/БИК {{bik}}</span></p>
                        <p><span>ЦБ № {{id}}</span></p>
                    </td>
                    <td colspan="5" style="width:207.25pt; vertical-align:top;">
                        <p><span>Мекен -жай:</span></p>
                        <p><span>{{locationKAZ}}</span></p>
                        <p><span>Адрес:</span></p>
                        <p><span>{{locationRUS}}</span></p>
                    </td>
                </tr>
                <tr class="inner" style="height:23.65pt;">
                    <td colspan="2" style="width:86.75pt;  vertical-align:middle;">
                        <p><span>Клиенттің атауы / Наименование клиента</span></p>
                    </td>
                    <td colspan="3" style="width:152.2pt; vertical-align:middle;">
                        <p><strong><span>{{name}}</span></strong></p>
                    </td>
                    <td colspan="2" style="width:87.55pt; vertical-align:middle;">
                        <p><span>Шот нөмірі / № счета</span></p>
                    </td>
                    <td colspan="3" style="width:108.9pt; vertical-align:middle;">
                        <p><strong><span>{{account}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:21.2pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>СНН / ИИН</span></p>
                        <p><span>(БСН / БИН)</span></p>
                    </td>
                    <td style="width:53pt; vertical-align:middle;">
                        <p><strong><span>{{bin}}</span></strong></p>
                    </td>
                    <td style="width:45.9pt; vertical-align:middle;">
                        <p><span>АЖК / КОД</span></p>
                    </td>
                    <td style="width:31.7pt; vertical-align:middle;">
                        <p><strong><span>{{code}}</span></strong></p>
                    </td>
                    <td style="width:53pt; vertical-align:middle;">
                        <p><span>БеК / КБе</span></p>
                    </td>
                    <td style="width:23.75pt; vertical-align:middle;">
                        <p><strong><span>{{kbe}}</span></strong></p>
                    </td>
                    <td colspan="2" style="width:49pt; vertical-align:middle;">
                        <p><span>ТБК / КНП</span></p>
                    </td>
                    <td style="width:49.1pt; vertical-align:middle;">
                        <p><strong><span>{{knp}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:21.45pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Қатысушының ТАӘ /</span></p>
                        <p><span>ФИО вносителя</span></p>
                    </td>
                    <td colspan="3" style="width:152.2pt; vertical-align:middle;">
                        <p><strong><span>{{lastName}} {{firstName}} {{middleName}}</span></strong></p>
                    </td>
                    <td style="width:53pt; vertical-align:middle;">
                        <p><span>СНН / ИИН</span></p>
                    </td>
                    <td colspan="4" style="width:143.45pt; vertical-align:middle;">
                        <p><strong><span>{{iin}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:13.3pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Ұсынылды / Предъявлен</span></p>
                    </td>
                    <td colspan="8" style="width:370.25pt; vertical-align:middle;">
                        <p><strong><span>{{documentType}} № {{documentNumber}} от {{documentGivenDate}}г. {{documentGivenBy}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:11.25pt;">
                    <td style="width:37.95pt; vertical-align:middle;">
                        <p><span>Валюта</span></p>
                    </td>
                    <td style="width:38pt; vertical-align:middle;">
                        <p><strong><span>{{currency}}</span></strong></p>
                    </td>
                    <td colspan="3" style="width:152.2pt; vertical-align:middle;">
                        <p><span>Сомасы санмен / Сумма цифрами</span></p>
                    </td>
                    <td colspan="5" style="width:207.25pt; vertical-align:middle;">
                        <p><strong>{{amount}}</strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:11.25pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Cомасы жазумен / Сумма прописью</span></p>
                    </td>
                    <td colspan="8" style="width:370.25pt; vertical-align:middle;">
                        <p><strong><span>{{amountStringKAZ}}</span></strong></p>
                        <p><strong><span>{{amountStringRUS}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:11.25pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Назначение / Мақсаты</span></p>
                    </td>
                    <td colspan="8" style="width:370.25pt; vertical-align:middle;">
                        <p><strong><span>{{goalKAZ}}</span></strong></p>
                        <p><strong><span>{{goalRUS}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:14.4pt;">
                    <td colspan="2" rowspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Қатысушының қолы / Подпись вносителя</span></p>
                    </td>
                    <td colspan="3" rowspan="2" style="width:152.2pt; vertical-align:middle;">
                        <p><strong><span></span></strong></p>
                    </td>
                    <td colspan="3" style="width:88.45pt; vertical-align:middle;">
                        <p><span>Орындаушы /</span></p>
                        <p><span>Исполнитель</span></p>
                    </td>
                    <td colspan="2" rowspan="2" style="width:108pt; vertical-align:middle;">
                        <p><strong><span></span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:14.4pt;">
                    <td colspan="3" style="width:88.45pt; vertical-align:middle;">
                        <p><span>{{executorFIO}}</span></p>
                    </td>
                </tr>
                <tr class="inner" style="height:14.4pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>Бақылаушы / Контролер</span></p>
                    </td>
                    <td colspan="3" rowspan="2" style="width:152.2pt; vertical-align:middle;">
                        <p><strong><span></span></strong></p>
                    </td>
                    <td colspan="3" style="width:88.45pt; vertical-align:middle;">
                        <p><span>Кассир</span></p>
                    </td>
                    <td colspan="2" rowspan="2" style="width:108pt; vertical-align:middle;">
                        <p><strong><span></span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:14.4pt;">
                    <td colspan="2" style="width:86.75pt; vertical-align:middle;">
                        <p><span>{{contollerFIO}}</span></p>
                    </td>
                    <td colspan="3" style="width:88.45pt; vertical-align:middle;">
                        <p><span>{{cashierFIO}}</span></p>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</body>

</html>

Это html док, видишь тут значения переданные по ширине хардкодом, можешь их усножить на 1.1 полностью и     <div class="container" style="width:479.35pt;"> этот див тэг установить пряимо по середине страницы
