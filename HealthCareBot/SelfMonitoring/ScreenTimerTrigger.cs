using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SelfMonitoring.Helper;
using SelfMonitoring.Model;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace SelfMonitoring
{
    public static class ScreenTimerTrigger
    {
        private static GraphServiceClient _graphServiceClient;

        [FunctionName("ScreenTimerTrigger")]
        public static void Run([TimerTrigger("0 0 0 1 1 *")]TimerInfo myTimer, ILogger log)
        {
            Task<List<string>> taskResult = Task.Run(GetMemberList);
            List<string> result = taskResult.Result;
            ProgrammaticTrigger.GetTeamsAddressFromSqlAndPostScreenTrigger(result);
        }

        private static async Task<List<string>> GetMemberList()
        {
            GraphServiceClient graphClient = GetAuthenticatedGraphClient();
            string groupId = System.Environment.GetEnvironmentVariable("GroupId", EnvironmentVariableTarget.Process);
            string[] groupIdList = groupId.Split(',');
            List<string> memberList = new List<string>();
            foreach (string item in groupIdList)
            {
                try
                {
                    var groupMembers = await graphClient.Groups[item].Members.Request().GetAsync();
                    PageIterator<DirectoryObject> groupMemberPageIterator = PageIterator<DirectoryObject>.CreatePageIterator(
                      graphClient,
                      groupMembers,
                      entity => { memberList.Add(entity.Id); return true; });
                    await groupMemberPageIterator.IterateAsync();
                }
                catch (Exception ex)
                {
                    memberList = null;
                }
            }

            //Get unique member ID list
            memberList = memberList.Distinct().ToList();
            return memberList;
        }        

        #region Graph SDK functions
        private static GraphServiceClient GetAuthenticatedGraphClient()
        {
            var authenticationProvider = CreateAuthorizationProvider();
            _graphServiceClient = new GraphServiceClient(authenticationProvider);
            return _graphServiceClient;
        }

        private static IAuthenticationProvider CreateAuthorizationProvider()
        {
            var clientId = System.Environment.GetEnvironmentVariable("AzureADAppClientId", EnvironmentVariableTarget.Process);
            var clientSecret = System.Environment.GetEnvironmentVariable("AzureADAppClientSecret", EnvironmentVariableTarget.Process);
            var redirectUri = System.Environment.GetEnvironmentVariable("AzureADAppRedirectUri", EnvironmentVariableTarget.Process);
            var tenantId = System.Environment.GetEnvironmentVariable("AzureADAppTenantId", EnvironmentVariableTarget.Process);
            var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
            
            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                                              .WithAuthority(authority)
                                              .WithRedirectUri(redirectUri)
                                              .WithClientSecret(clientSecret)
                                              .Build();

            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
        #endregion
    }
}
