Тип System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] не поддерживается, т.к. он реализует IDictionary.


        [WebMethod]
        public Pair DublicateToUnderLimits(Dictionary<string, List<string>> dict, string type)
        {
            Pair p = new Pair() { isErr = false, Msg = "" };
            try
            {
                new MSBDocumentsPocketController().DublicateToUnderLimits(dict, type);
            }
            catch (Exception ex)
            {
                p.isErr = true;
                p.Msg = ex.ToString();
            }
            return p;
        }
