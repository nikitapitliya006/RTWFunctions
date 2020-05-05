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

namespace SelfMonitoring
{
    public static class GetScreeningInfo
    {
        [FunctionName("GetScreeningInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetScreeningInfo/{UserId}")] HttpRequest req, string UserId,
            ILogger log, ExecutionContext context)
        {
            //Guid pID = patientID;
            if (UserId == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }


            ScreeningInfo screeningInfo = await DbHelper.GetDataAsync<ScreeningInfo>(context,
                                                                                     Constants.getScreeningInfo,
                                                                                     UserId);
            
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
    }
}
