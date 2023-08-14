using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ConsoleApp;
using System.IO;
using System.Text;

namespace PostConsole
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            string[] iins = File.ReadAllLines("C:/Users/00055864/source/repos/ConsoleApp/iin.txt");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            TemplateBody body = new TemplateBody()
            {
                contractNumbers = null,
                dateFrom = "2022-05-21",
                dateTo = "2023-05-21",
                onlyIncomes = true
            };
            Accounts accounts = new Accounts();
            foreach (string iin in iins) 
            {
                string jsonString = $"{{ \"iin\" : \"{iin}\" }}";
                string acc = await GetAccount(client, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                accounts = JsonConvert.DeserializeObject<Accounts>(acc);
                body.contractNumbers = accounts.targetNumbers;
                await SendPostRequest(client, iin, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            }
        }

        static async Task<string> GetAccount (HttpClient client, StringContent iin)
        {
            try
            {
                var url = "http://halykbpm-dev-core.homebank.kz/card-transactions/get-accounts";
                var response = await client.PostAsync(url, iin);
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
        }

        static async Task SendPostRequest(HttpClient client, string iin, StringContent data)
        {
            try
            {
                var url = "https://halykbpm-dev-integration.halykbank.nb/way4-client-info-integration/contract/incomes";
                var response = await client.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                File.WriteAllText("C:/Users/00055864/source/repos/ConsoleApp/results/" + iin + ".json", result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught! Message :{0} ", e.Message);
            }
        }
    }
}
