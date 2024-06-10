       
        private string ReplaceFields(T model, string content)   Я пытаюсь написать метод, который будет автоматом заменять все поля, на основе модели.
        {
            PropertyInfo[] fields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo field in fields) 
            {
                var fieldValue = field.GetValue(model);
                var filedName = field.Name;
                if (field.PropertyType.IsClass && field.PropertyType.Name == QrCode) 
                {
                    content.Replace($"{{{filedName}}}", _generator.GenerateQrCodeBase64(fieldValue)); 
                }
                content.Replace($"{{{filedName}}}", fieldValue); // получаю ошибку из за скобок, как можно лучше реализовать все это, также если 
            }
        }
