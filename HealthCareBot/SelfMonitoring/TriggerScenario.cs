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
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
            //log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            
            GraphServiceClient graphClient = GetAuthenticatedGraphClient();            

            string groupId = "a72cc7c9-1bbe-42fc-8ddb-60e5aad2dccb";
            
            List<string> groupList = new List<string>();
            for (int i = 0; i < groupList.Count; i++)
            {
                //groupMembers = await graphClient.Groups[groupList[i]].Members.Request().GetAsync();
            }
            //var groupMembers = await graphClient.Groups["a72cc7c9-1bbe-42fc-8ddb-60e5aad2dccb"].Members.Request().GetAsync();
            var groupMembers = await graphClient.Groups[groupId].Members.Request().GetAsync();

            List<string> memberList = new List<string>();
            for (int i = 0; i < groupMembers.Count; i++)
            {
                memberList.Add(groupMembers.CurrentPage[i].Id);
            }

            //To-do: Build SQL string with dynamic list of memberIDs - Get all TeamsAddress using single sql query
            //var sql_1 = "select ui.UserId, ui.TeamsAddress, si.QuarantineRequired from UserInfo as ui left join ScreeningInfo as si on(si.UserId = ui.UserId) where ui.UserId IN(";
            //var sql_2 = '1612a50c-c637-4f41-ba32-9f1bd1c5aead','4dbcaaa2-3258-4917-bcea-4049981dc704'
            //var sql_3 = ") AND si.QuarantineRequired = 0"

            //Calling Azure function to get UserInfo table details 
            List<string> teamsAddressList = new List<string>();
            for (int i = 0; i < memberList.Count; i++)
            {
                string address1 = GetTeamsAddressForEachUser(memberList[i]);
                teamsAddressList.Add(address1);
            }

            //Post Trigger covid19_screen scenario
            foreach (var addr in teamsAddressList)
            {
                PostScreenTrigger(addr);
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        static async void PostScreenTrigger(string teamsAddress)
        {
            const string URL = "https://bot-us.healthbot.microsoft.com/api/tenants/contosohealthsystemteamsbot-g4ubxvv/beginScenario";
            
            string token = "";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            //Add an Authorization Bearer token (jwt token)
            string partial_token = GetJwtToken();
            token = "Bearer " + partial_token;

            //token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0ZW5hbnROYW1lIjoiY29udG9zb2hlYWx0aGJvdC1pamhtamR5IiwiaWF0IjoxNTg4NDkwOTcxfQ.cIiM7BbiaY3oF1eDqMMiSFGgaaXteyZu1yk-E36Ucmw";
            client.DefaultRequestHeaders.Add("Authorization", token);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Add body parameter
            var payload = "{\"address\":" + teamsAddress + ",\"scenario\": \"/scenarios/screen\"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = await client.PostAsync(URL, content);
            
            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                Console.WriteLine("{0})", (int)response.StatusCode);
            }

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

        }

        #region jwttoken
        
        public static string GetJwtToken()
        {
            var healthbot_API_JWT_SECRET = "0e55744e1c921be33ab9020ab38e9bf80bb29ad7b60188424b89b0614ccb5271";
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            // An alternative option is used here
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(healthbot_API_JWT_SECRET));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //  Finally create a Token
            var header = new JwtHeader(signingCredentials);

            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload
            {
               { "tenantName", "contosohealthsystemteamsbot-g4ubxvv"},
               { "iat", secondsSinceEpoch},
            };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);
            //Console.WriteLine("\nEncoded: \n" + tokenString);

            //var decodedToken = handler.ReadToken(tokenString);
            //Console.WriteLine("\nDecoded: \n" + decodedToken);

            return tokenString;
        }

        #endregion
        private static string GetTeamsAddressForEachUser(string memberID)
        {
            const string URL = "https://selfmonitoring.azurewebsites.net/api/GetUserInfo/";
            //string userID = memberID;
            string userID = "1612a50c-c637-4f41-ba32-9f1bd1c5aead";
            string teamsAddress = "todo";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(userID).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body                
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                teamsAddress = JsonConvert.DeserializeObject<Model.UserInfo>(jsonString.Result).TeamsAddress;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();

            //To-do: Return a list of member's Teams Address from DB

            return teamsAddress;
        }

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

            return new Helper.MsalAuthenticationProvider(cca, scopes.ToArray());
        }
    }
}
