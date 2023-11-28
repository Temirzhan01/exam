        public object Deserialize(Type type, string loginFolder, string RequestNumber)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            StreamReader reader = new StreamReader(@"C:/myDataSource/MSB/Front/" + loginFolder + "/" + RequestNumber + "/BaseViewModel.xml");
            object _object = serializer.Deserialize(reader.BaseStream);
            reader.Close();
            return _object;
        }
