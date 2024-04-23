{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AppSettings": {
    "ExternalServices": {
      "Camunda": ""
    },
    "Kafka": {
      "Topic": "",
      "GroupId": "",
      "BootstrapServers": "",
      "SaslUsername": "",
      "SaslPassword": ""
    },
    "BKGeneratorType": "",
    "ProcessName": "",
    "Classes": ""  //как сюда вставлять массив стрингов? типа ["str1","str2"]?
  }
}


namespace LegalCashOperationsWorker.Models
{
    public class AppSettings 
    {
        public KafkaSettings KafkaSettings { get; set; }
        public ExternalServices ExternalServices { get; set; }
        public string BKGeneratorType { get; set; }
        public string ProcessName { get; set; }
        public string[] Classes { get; set; }
    }

    public class KafkaSettings
    {
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
    }

    public class ExternalServices
    {
        public string Camunda {  get; set; }
    }

}
