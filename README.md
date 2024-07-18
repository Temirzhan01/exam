using Consul;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            LoadJsonConfiguration(kvp.Value);
        }
    }

    private async Task<Dictionary<string, string>> GetConsulData(ConsulClient client, string key)
    {
        var result = new Dictionary<string, string>();
        var queryResult = await client.KV.Get(key);

        if (queryResult.Response != null)
        {
            string consulValue = Encoding.UTF8.GetString(queryResult.Response.Value);
            result[key] = consulValue;
        }

        return result;
    }

    private void LoadJsonConfiguration(string json)
    {
        var jObject = JObject.Parse(json);
        foreach (var kvp in jObject)
        {
            ParseJson(kvp, string.Empty);
        }
    }

    private void ParseJson(KeyValuePair<string, JToken> kvp, string parentPath)
    {
        var currentPath = parentPath == string.Empty ? kvp.Key : $"{parentPath}:{kvp.Key}";

        if (kvp.Value.Type == JTokenType.Object)
        {
            foreach (var childKvp in (JObject)kvp.Value)
            {
                ParseJson(childKvp, currentPath);
            }
        }
        else
        {
            Data[currentPath] = kvp.Value.ToString();
        }
    }
}
