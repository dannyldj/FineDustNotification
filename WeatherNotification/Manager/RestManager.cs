using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherNotification.Model;
using WeatherNotification.Properties;

namespace WeatherNotification.Manager
{
    public class RestManager
    {
        /// <summary>
        /// 서버 통신
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> 서버 통신의 결과값 </returns>
        public async Task<T> RestRequest<T>(string uri, List<QParamModel> queryParams, string reosurces)
        {
            var restClient = new RestClient(uri);
            var request = new RestRequest(reosurces, Method.GET);

            if (queryParams != null)
            {
                foreach (QParamModel queryParam in queryParams)
                {
                    request.AddQueryParameter(queryParam.Key, queryParam.Value);
                }
            }

            // await = 위의 비동기 작업이 끝날때 까지 기다려줌
            var response = await restClient.ExecuteAsync<T>(request);

            var result = JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }
    }
}
