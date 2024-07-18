Вот точка входа в метод, загрузки конфигурации из консула в Program.cs

  builder.AddConsulConfiguration();

И файл appsettings локальный со ссылкой на консул и названием окружения откуда нужно  брать конфиги 
{
  "Host": "наша ссылка",
  "Environment": "название окружения"
}


Далее это твой код 

      public static class ConsulConfigurationExtensions
    {
        public static WebApplicationBuilder AddConsulConfiguration(this WebApplicationBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            var builtConfig = configBuilder.Build();
            configBuilder.Add(new ConsulConfigurationSource(builtConfig["Host"], builtConfig["Environment"]));

            var config = configBuilder.Build();
            builder.Configuration.AddConfiguration(config); // тут при добавлении вижу что они получены с консула как провайдер - {ConsulConfigurationProvider} в поле Data есть один ключ название окружения Environment и просто json строка из нужных мне конфигов, к которым я могу обратиться только так _configuration["SstlService"] и то в виде json файла, а мне нужны ключ значения

            return builder;
        }
    }

    public class ConsulConfigurationSource : IConfigurationSource
    {
        private readonly string _consulAddress;
        private readonly string _key;

        public ConsulConfigurationSource(string consulAddress, string key)
        {
            _consulAddress = consulAddress;
            _key = key;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(_consulAddress, _key);
        }
    }


    и это также твой код

          public class ConsulConfigurationProvider : ConfigurationProvider
    {
        private readonly string _consulAddress;
        private readonly string _key;

        public ConsulConfigurationProvider(string consulAddress, string key)
        {
            _consulAddress = consulAddress;
            _key = key;
        }

        public override void Load()
        {
            var client = new ConsulClient(config =>
            {
                config.Address = new Uri(_consulAddress);
            });

            Task<Dictionary<string, string>> getConsulData = GetConsulData(client, _key);
            getConsulData.Wait();

            foreach (var kvp in getConsulData.Result)
            {
                Data.Add(kvp.Key, kvp.Value); // тут kvp.Value в виде { "ConnectionStrings": {  "Cardcolv": "" },"ApiKey": "" } в виде конфигурационного файла.

            }
        }

        private async Task<Dictionary<string, string>> GetConsulData(ConsulClient client, string key)
        {
            var result = new Dictionary<string, string>();
            var queryResult = await client.KV.Get(key);

            if (queryResult.Response != null)
            {
                string consulValue = System.Text.Encoding.UTF8.GetString(queryResult.Response.Value);
                result[key] = consulValue;
            }

            return result;
        }
    }
Короче, как то не корректно получает с консула, и забудь про синглтон класс 
