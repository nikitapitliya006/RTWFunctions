using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using SelfMonitoring.Model;
using System.Data.SqlClient;
using System.Text;
using SelfMonitoring.Helper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SelfMonitoring
{
    public static class GetScreeningInfo
    {
        [FunctionName("GetScreeningInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetScreeningInfo/{UserId}")] HttpRequest req, string UserId,
            ILogger log, ExecutionContext context)
        {           
            if (UserId == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                List<ScreeningInfo> screeningInfo = await DbHelper.GetDataAsync<List<ScreeningInfo>>(Constants.getScreeningInfo, UserId);
                if (screeningInfo == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                log.LogInformation(JsonConvert.SerializeObject(screeningInfo));

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(screeningInfo), Encoding.UTF8, "application/json")
                };
            }
            catch (System.Exception ex)
            {
                log.LogInformation(ex.Message);
                return null;
            }
        }
    }
}
