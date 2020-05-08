using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using SelfMonitoring.Model;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using SelfMonitoring.Helper;
using Microsoft.Extensions.Configuration;

namespace SelfMonitoring
{
    public static class GetQuarantineInfo
    {
        [FunctionName("GetQuarantineInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetQuarantineInfo/{UserId}")] HttpRequest req, string UserId,
            ILogger log, ExecutionContext context)
        {
            if (UserId == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var conStr = config["SqlConnectionString"];

            List<QuarantineInfo> ListqData = await DbHelper.GetDataAsync<List<QuarantineInfo>>(Constants.getQuarantineInfo, UserId);
            
            if (ListqData == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            log.LogInformation(JsonConvert.SerializeObject(ListqData));

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(ListqData), Encoding.UTF8, "application/json")
            };
        }
    }
}
