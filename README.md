using System.Net.Http.Headers;
using System.Text;

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
string[] values = new string[] {"string0", "string1", "string2", "string3", "string4", "string5", };

for (int i = 0; i < values.Length; i++) 
{
    var jsonTemplate = "{{\r\n  \"param\": \"{0}\",\r\n  \"param2\": {1}\r\n}}";
    var json = string.Format(jsonTemplate, values[i], i);
    var data = new StringContent(json, Encoding.UTF8, "application/json");
    await Request(client, data);
}


async Task Request(HttpClient client, StringContent data)
{
    var url = "https://localhost:7023/Home";
    var response = await client.PostAsync(url, data);
    string result = await response.Content.ReadAsStringAsync();
    Console.WriteLine(result);
}

