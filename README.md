<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <table>
        <tr>
            <th>{{Company}}</th>
            <th>{{Contact}}</th>
            <th>{{Country}}</th>
        </tr>
    </table>
    <img src="data:image/png;base64,{{qrCode}}" alt="QR Code" />
</body>
</html>

        private string ReplaceFields(T model, string content) 
        {
            PropertyInfo[] fields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo field in fields) 
            {
                var fieldValue = field.GetValue(model)?.ToString() ?? string.Empty;
                var filedName = field.Name;
                if (field.PropertyType.IsClass && field.PropertyType.Name == "QrCode")
                {
                    content.Replace($"{{{filedName}}}", _generator.GenerateQrCodeBase64(fieldValue));  //они не робят пля
                }
                else 
                {                
                    content.Replace($"{{{filedName}}}", fieldValue); //они не робят пля
                }
            }
            return content;
        }
