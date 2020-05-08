using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using SelfMonitoring.Model;
using System.Data.SqlClient;
using SelfMonitoring.Helper;

namespace SelfMonitoring
{
    public static class PostScreeningInfo
    {
        [FunctionName("PostScreeningInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
            ILogger log, ExecutionContext context)
        {
            ScreeningInfo screeningInfo = await req.Content.ReadAsAsync<ScreeningInfo>();
            if (screeningInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            bool dataRecorded = await DbHelper.PostDataAsync(screeningInfo, Constants.postScreeningInfo);

            if (dataRecorded)
            {
                log.LogInformation("Data recorded...");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }            
        }
    }
}
