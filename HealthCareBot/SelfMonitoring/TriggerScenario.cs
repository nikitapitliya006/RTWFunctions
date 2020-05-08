using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
//using System.IdentityModel.Tokens;
//using System.Security.Claims;
//using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SelfMonitoring.Helper;
using SelfMonitoring.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SelfMonitoring
{
    public static class TriggerScenario
    {
        private static GraphServiceClient _graphServiceClient;

        [FunctionName("TriggerScenario")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

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

            //Check: backend DB = FHIR or Azure SQL
            string SqlConnectionString = System.Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);
            if (SqlConnectionString != null)
            {
                GetTeamsAddressFromSqlAndPostTrigger(memberList);
            }
            else
            {
                GetTeamsAddressFromFhirAndPostTrigger(memberList);
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        private static async void GetTeamsAddressFromSqlAndPostTrigger(List<string> members)
        {
            //Get Teams Address 
            List<TeamsAddressQuarantineInfo> teamsAddressQuarantineInfoCollector = new List<TeamsAddressQuarantineInfo>();
            bool result = await DbHelper.GetTeamsAddress(members, teamsAddressQuarantineInfoCollector);
            Console.WriteLine(JsonConvert.SerializeObject(teamsAddressQuarantineInfoCollector));

            //Post Trigger "screen" scenario
            foreach (var element in teamsAddressQuarantineInfoCollector)
            {
                PostScreenTrigger(element.TeamsAddress);
            }
        }

        private static void GetTeamsAddressFromFhirAndPostTrigger(List<string> members)
        {
            ////Get Teams Address

            ////Post Trigger "screen" scenario
            //foreach (var element in teamsAddressQuarantineInfoCollector)
            //{
            //    PostScreenTrigger(element.TeamsAddress);
            //}
        }

        #region PostTrigger with Jwttoken
        static async void PostScreenTrigger(string teamsAddress)
        {
            string URL = System.Environment.GetEnvironmentVariable("Healthbot_Trigger_Call", EnvironmentVariableTarget.Process);
            string token = "";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            //Add an Authorization Bearer token (jwt token)
            string partial_token = GetJwtToken();
            token = "Bearer " + partial_token;
            client.DefaultRequestHeaders.Add("Authorization", token);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Add body parameter
            string scenarioId = System.Environment.GetEnvironmentVariable("Healthbot_ScenarioId_Screening", EnvironmentVariableTarget.Process);
            var payload = "{\"address\":" + teamsAddress + ",\"scenario\": \"/scenarios/" + scenarioId + "\"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = await client.PostAsync(URL, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0}", (int)response.StatusCode);
            }
            client.Dispose();
        }

        public static string GetJwtToken()
        {
            var healthbot_API_JWT_SECRET = System.Environment.GetEnvironmentVariable("Healthbot_API_JWT_SECRET", EnvironmentVariableTarget.Process);
            var healthbot_Tenant_Name = System.Environment.GetEnvironmentVariable("Healthbot_Tenant_Name", EnvironmentVariableTarget.Process);
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(healthbot_API_JWT_SECRET));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            //var payload = new JwtPayload
            //{
            //   { "tenantName", "contosohealthsystemteamsbot-g4ubxvv"},
            //   { "iat", secondsSinceEpoch},
            //};


            var payload = new JwtPayload
            {
               { "tenantName", "" + healthbot_Tenant_Name + ""},
               { "iat", secondsSinceEpoch},
            };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken);
            return tokenString;
        }

        #endregion

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

            //this specific scope means that application will default to what is defined in the application registration rather than using dynamic scopes
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
