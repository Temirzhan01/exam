using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PostConsole
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string template = @"{ ""contractNumber"":[""KZ756010002013215255""], ""dateFrom"":""2022-05-21"", ""dateTo"":""2023-05-21"", ""onlyIncomes"": true }";
            string template = @"{
            'contractNumber':['KZ756010002013215255'],
            'dateFrom':'2022-05-21',
            'dateTo':'2023-05-21',
            'onlyIncomes': true
            }";
            string data = JsonConvert.SerializeObject(template);
            await SendPostRequest(client, new StringContent(data, Encoding.UTF8, "application/json"));
        }
        static async Task SendPostRequest(HttpClient client, StringContent data)
        {
            try
            {
                var url = "https://halykbpm-dev-integration.halykbank.nb/way4-client-info-integration/contract/incomes";
                var response = await client.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught! Message :{0} ", e.Message);
            }
        }
    }
}
