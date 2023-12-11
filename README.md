using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using CLASB.Models;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Web.Services;

namespace MSBWS
{
    public partial class MBWS
    {
        [WebMethod(Description = "ЦАЗ выдача, по заявка с ОБ")]
        public string CamundaOBRequest(string type, string body)
        {
            _logger.Info($"Request type: {type}, request body: {body}");
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    switch (type)
                    {
                        case "GET":
                            return JsonConvert.SerializeObject(GetCamundaOBId(body));
                        case "POST":
                            CamundaOBIdStatus camundaOBIdStatus = JsonConvert.DeserializeObject<CamundaOBIdStatus>(body);
                            return JsonConvert.SerializeObject(PostCamundaObStatus(camundaOBIdStatus));
                        case "EXIST":
                            return ProcExistInOb(body);
                        default:
                            throw new HttpRequestException("Not valid parameter type");
                    }
                }
                throw new NullReferenceException("Null parameter type");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception: {ex}");
                throw;
            }
        }

        public CamundaOBId GetCamundaOBId(string bin)
        {
            try
            {
                var response = HttpClientService._client.GetAsync($"/proclist?bin={bin}");
                response.Result.EnsureSuccessStatusCode();
                string strResponse = response.Result.Content.ReadAsStringAsync().Result.ToString();
                if (!string.IsNullOrEmpty(strResponse) && strResponse != "null")
                {
                    List<CamundaOBId> result = JsonConvert.DeserializeObject<List<CamundaOBId>>(strResponse);
                    return result[0];
                }
                throw new HttpRequestException();
            }
            catch
            {
                throw;
            }
        }

        public CamundaOBResult PostCamundaObStatus(CamundaOBIdStatus camundaOBIdStatus)
        {
            try
            {
                var response = HttpClientService._client.PostAsync("/setCAZstatus", new StringContent(JsonConvert.SerializeObject(camundaOBIdStatus), Encoding.UTF8, "application/json"));
                response.Result.EnsureSuccessStatusCode();
                CamundaOBResult result = JsonConvert.DeserializeObject<CamundaOBResult>(response.Result.Content.ReadAsStringAsync().Result.ToString());
                if (result.code != "0")
                {
                    throw new Exception(result.message);
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public string ProcExistInOb(string bin)
        {
            try
            {
                var response = HttpClientService._client.GetAsync($"/procExistInOb?bin={bin}");
                response.Result.EnsureSuccessStatusCode();
                return response.Result.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }
    }
    public static class HttpClientService
    {
        public static readonly HttpClient _client;
        static HttpClientService()
        {
            var handler = new HttpClientHandler()
            {
                Proxy = new WebProxy(),
                UseDefaultCredentials = true
            };
            _client = new HttpClient(handler) { MaxResponseContentBufferSize = int.MaxValue, BaseAddress = new Uri(ConfigurationManager.AppSettings["CamundaOnlineBank"]) };
        }
    }
}

