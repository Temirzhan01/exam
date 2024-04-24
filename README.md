            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo field in fields) 
            {
                var fieldValue = field.GetValue(obj);
                string fieldType = field.FieldType.Name;
                if (field.FieldType.IsClass && field.FieldType != typeof(string)) 
                {
                    fieldType = "json";
                }
                result.Add(field.Name, new Variable(fieldType, fieldValue));
            }


            field.Name выдает не только название, но и доп. <name>Back че то там, как получить чисто само название? 
