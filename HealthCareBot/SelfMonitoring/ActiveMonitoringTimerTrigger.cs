using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SelfMonitoring
{
    public static class ActiveMonitoringTimerTrigger
    {
        [FunctionName("ActiveMonitoringTimerTrigger")]
        public static void Run([TimerTrigger("0 0 8,14 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
