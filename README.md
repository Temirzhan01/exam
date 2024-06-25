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
            padding-right: 5.53pt;
            padding-left: 5.53pt;
        }

        .container {
            width: 527.29pt;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>

<body>
    <div class="container">
        <div class="header">
            <p style="text-align:center; font-size:10pt;"><strong><span>Кассалық кіріс ордері / Приходный кассовый ордер № 1 от {{date}} г.</span></strong></p>
            <p style="text-align:right; font-size:8pt;"><span>Уақыты / Время: {{time}}</span></p>
        </div>
        <table class="content" cellspacing="0" cellpadding="0" style="width:527.29pt; border-collapse:collapse;">
            <tbody>
                <tr style="height:35.64pt;">
                    <td colspan="5" style="width:274.73pt; vertical-align:middle;">
                        <p><span>{{nameKAZ}}</span></p>
                        <p><span>{{nameRUS}}</span></p>
                        <p><span>БСН / БИН {{bin}}</span></p>
                        <p><span>ББК/БИК {{bik}}</span></p>
                        <p><span>ЦБ № {{id}}</span></p>
                    </td>
                    <td colspan="5" style="width:227.98pt; vertical-align:top;">
                        <p><span>Мекен -жай:</span></p>
                        <p><span>{{locationKAZ}}</span></p>
                        <p><span>Адрес:</span></p>
                        <p><span>{{locationRUS}}</span></p>
                    </td>
                </tr>
                <tr class="inner" style="height:26.02pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Клиенттің атауы / Наименование клиента</span></p>
                    </td>
                    <td colspan="3" style="width:167.42pt; vertical-align:middle;">
                        <p><strong><span>{{name}}</span></strong></p>
                    </td>
                    <td colspan="2" style="width:96.31pt; vertical-align:middle;">
                        <p><span>Шот нөмірі / № счета</span></p>
                    </td>
                    <td colspan="3" style="width:119.79pt; vertical-align:middle;">
                        <p><strong><span>{{account}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:23.32pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>СНН / ИИН</span></p>
                        <p><span>(БСН / БИН)</span></p>
                    </td>
                    <td style="width:58.3pt; vertical-align:middle;">
                        <p><strong><span>{{bin}}</span></strong></p>
                    </td>
                    <td style="width:50.49pt; vertical-align:middle;">
                        <p><span>АЖК / КОД</span></p>
                    </td>
                    <td style="width:34.87pt; vertical-align:middle;">
                        <p><strong><span>{{code}}</span></strong></p>
                    </td>
                    <td style="width:58.3pt; vertical-align:middle;">
                        <p><span>БеК / КБе</span></p>
                    </td>
                    <td style="width:26.13pt; vertical-align:middle;">
                        <p><strong><span>{{kbe}}</span></strong></p>
                    </td>
                    <td colspan="2" style="width:53.9pt; vertical-align:middle;">
                        <p><span>ТБК / КНП</span></p>
                    </td>
                    <td style="width:53.91pt; vertical-align:middle;">
                        <p><strong><span>{{knp}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:23.45pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Қатысушының ТАӘ /</span></p>
                        <p><span>ФИО вносителя</span></p>
                    </td>
                    <td colspan="3" style="width:167.42pt; vertical-align:middle;">
                        <p><strong><span>{{lastName}} {{firstName}} {{middleName}}</span></strong></p>
                    </td>
                    <td style="width:58.3pt; vertical-align:middle;">
                        <p><span>СНН / ИИН</span></p>
                    </td>
                    <td colspan="4" style="width:157.80pt; vertical-align:middle;">
                        <p><strong><span>{{iin}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:14.63pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Ұсынылды / Предъявлен</span></p>
                    </td>
                    <td colspan="8" style="width:407.28pt; vertical-align:middle;">
                        <p><strong><span>{{documentType}} № {{documentNumber}} от {{documentGivenDate}}г. {{documentGivenBy}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:12.38pt;">
                    <td style="width:41.75pt; vertical-align:middle;">
                        <p><span>Валюта</span></p>
                    </td>
                    <td style="width:41.80pt; vertical-align:middle;">
                        <p><strong><span>{{currency}}</span></strong></p>
                    </td>
                    <td colspan="3" style="width:167.42pt; vertical-align:middle;">
                        <p><span>Сомасы санмен / Сумма цифрами</span></p>
                    </td>
                    <td colspan="5" style="width:227.98pt; vertical-align:middle;">
                        <p><strong>{{amount}}</strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:12.38pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Cомасы жазумен / Сумма прописью</span></p>
                    </td>
                    <td colspan="8" style="width:407.28pt; vertical-align:middle;">
                        <p><strong><span>{{amountStringKAZ}}</span></strong></p>
                        <p><strong><span>{{amountStringRUS}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:12.38pt;">
                    <td colspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Назначение / Мақсаты</span></p>
                    </td>
                    <td colspan="8" style="width:407.28pt; vertical-align:middle;">
                        <p><strong><span>{{goalKAZ}}</span></strong></p>
                        <p><strong><span>{{goalRUS}}</span></strong></p>
                    </td>
                </tr>
                <tr class="inner" style="height:15.84pt;">
                    <td colspan="2" rowspan="2" style="width:95.43pt; vertical-align:middle;">
                        <p><span>Қатысушының қолы / Подпись вносителя</span></p>
                    </td>
                    <td colspan="3" rowspan="2" style="width:167.42pt; vertical-align:middle;">
                        <p><strong><span></span></strong></p>
                    </td>
                    <td colspan="3" style="width:97.30pt; vertical-align:middle;">
                        <p><span>Орындаушы /</span></p>
                        <p><span>Исполнитель</span></p>
</td>
<td colspan="2" rowspan="2" style="width:118.80pt; vertical-align:middle;">
<p><strong><span></span></strong></p>
</td>
</tr>
<tr class="inner" style="height:15.84pt;">
<td colspan="3" style="width:97.30pt; vertical-align:middle;">
<p><span>{{executorFIO}}</span></p>
</td>
</tr>
<tr class="inner" style="height:15.84pt;">
<td colspan="2" style="width:95.43pt; vertical-align:middle;">
<p><span>Бақылаушы / Контролер</span></p>
</td>
<td colspan="3" rowspan="2" style="width:167.42pt; vertical-align:middle;">
<p><strong><span></span></strong></p>
</td>
<td colspan="3" style="width:97.30pt; vertical-align:middle;">
<p><span>Кассир</span></p>
</td>
<td colspan="2" rowspan="2" style="width:118.80pt; vertical-align:middle;">
<p><strong><span></span></strong></p>
</td>
</tr>
<tr class="inner" style="height:15.84pt;">
<td colspan="2" style="width:95.43pt; vertical-align:middle;">
<p><span>{{contollerFIO}}</span></p>
</td>
<td colspan="3" style="width:97.30pt; vertical-align:middle;">
<p><span>{{cashierFIO}}</span></p>
</td>
</tr>
</tbody>
</table>
</div>

</body>
</html>
