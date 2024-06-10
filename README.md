        public async Task<object> Create(string typeName) 
        {
            Type type = Type.GetType(typeName); //тут получаю null хотя параметр не пустой, и такой класс существуют в папке модели
            if (type == null) 
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            Type genericType = typeof(IDrawDocumentService<>).MakeGenericType(type); 
            return _serviceProvider.GetService(genericType);
        }
