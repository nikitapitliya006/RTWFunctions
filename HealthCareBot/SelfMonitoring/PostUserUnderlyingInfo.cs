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
    public static class PostUserUnderlyingInfo
    {
        [FunctionName("PostUserUnderlyingInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log, ExecutionContext context)
        {

            UserUnderlyingInfo userUnderlyingInfo = await req.Content.ReadAsAsync<UserUnderlyingInfo>();
            if (userUnderlyingInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            bool dataRecorded = await DbHelper.PostDataAsync(context, userUnderlyingInfo, Constants.postUserUnderlyingInfo);

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
