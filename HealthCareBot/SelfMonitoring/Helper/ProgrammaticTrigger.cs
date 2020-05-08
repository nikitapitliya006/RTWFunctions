using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SelfMonitoring.Model;

namespace SelfMonitoring.Helper
{
    public class ProgrammaticTrigger
    {
        #region Trigger Screen Scenario
        public static async void GetTeamsAddressFromSqlAndPostScreenTrigger(List<string> members)
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

        public static async void PostScreenTrigger(string teamsAddress)
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
                Console.WriteLine("{0})", (int)response.StatusCode);
            }
            client.Dispose();
        }

        #endregion

        #region Triggering Active Monitoring
        public static async void GetTeamsAddressFromSqlAndPostAMATrigger(List<string> members)
        {
            //Get Teams Address 
            List<TeamsAddressQuarantineInfo> teamsAddressQuarantineInfoCollector = new List<TeamsAddressQuarantineInfo>();
            bool result = await DbHelper.GetTeamsAddress(members, teamsAddressQuarantineInfoCollector);
            Console.WriteLine(JsonConvert.SerializeObject(teamsAddressQuarantineInfoCollector));

            //Post Trigger "screen" scenario
            foreach (var element in teamsAddressQuarantineInfoCollector)
            {
                PostAMATrigger(element.TeamsAddress);
            }
        }

        public static async void PostAMATrigger(string teamsAddress)
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
            string scenarioId = System.Environment.GetEnvironmentVariable("Healthbot_ScenarioId_Monitoring", EnvironmentVariableTarget.Process);
            var payload = "{\"address\":" + teamsAddress + ",\"scenario\": \"/scenarios/" + scenarioId + "\"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            // List data response.
            HttpResponseMessage response = await client.PostAsync(URL, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0})", (int)response.StatusCode);
            }
            client.Dispose();
        }

        #endregion

        public static string GetJwtToken()
        {
            var healthbot_API_JWT_SECRET = System.Environment.GetEnvironmentVariable("Healthbot_API_JWT_SECRET", EnvironmentVariableTarget.Process);
            var healthbot_Tenant_Name = System.Environment.GetEnvironmentVariable("Healthbot_Tenant_Name", EnvironmentVariableTarget.Process);
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(healthbot_API_JWT_SECRET));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
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
    }    
}
