  "error": "Index and length must refer to a location within the string. (Parameter 'length')"
        public async Task<TransactionInformationShort> GetTransactionInformation(string transactionCode)
        {
            return await serviceSoap.GetTransactionInformationAsync(transactionCode.Substring(0, transactionCode.Length - 6), transactionCode.Substring(transactionCode.Length - 6, transactionCode.Length));
        }
