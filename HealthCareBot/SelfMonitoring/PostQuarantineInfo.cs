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
    public static class PostQuarantineInfo
    {
        [FunctionName("PostQuarantineInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpResponseMessage req,
            ILogger log, ExecutionContext context)
        {
            try
            {
                QuarantineInfo quarantineInfo = await req.Content.ReadAsAsync<QuarantineInfo>();
                if (quarantineInfo == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                bool dataRecorded = await DbHelper.PostDataAsync(quarantineInfo, Constants.postQuarantineInfo);

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
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
