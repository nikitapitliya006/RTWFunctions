using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using HealthBotV4DataService.Data;
using QRCoder;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;


namespace HealthBotV4_DataService
{
    public static class SavePatientData
    {
        [FunctionName("SavePatientData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation("requestBody = {requestBody}");

            string finalGuid, responseMessage;

            string toBeSearched = "State\":\"";
            int ix = requestBody.IndexOf(toBeSearched);
            finalGuid = requestBody.Substring(ix + toBeSearched.Length, 36);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(finalGuid, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //Save in local folder
            //qrCodeImage.Save("C:\\Nikita\\HealthBot\\Integrations\\MyTestResult.jpg", ImageFormat.Jpeg);
            responseMessage = "QR code generated.";
            //convert bitmap to png in MemoryStream
            byte[] resultQR = null;
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                resultQR = stream.ToArray();
            }

            responseMessage = string.IsNullOrEmpty(requestBody)
                ? "This HTTP triggered function executed successfully. Data not recieved"
                : string.Empty;
            var data = JsonConvert.DeserializeObject<PatientData>(requestBody);
            if (data == null)
                responseMessage = "No patient data.";
            else
            {
                var optionsBuilder = new DbContextOptionsBuilder<HealthBotDBContext>();
                optionsBuilder.UseSqlServer("Server=tcp:kohli.public.a169846ab65d.database.windows.net,3342;Initial Catalog=HealthBotDB;Persist Security Info=False;User ID=prema;Password=prema;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", options => options.EnableRetryOnFailure());
                //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString, options => options.EnableRetryOnFailure());

                using (var dbContext = new HealthBotDBContext(optionsBuilder.Options))
                {
                    dbContext.PatientData.Add(data);
                    dbContext.SaveChanges();
                }

                //Add QRcoder code here
            }

            responseMessage = string.IsNullOrEmpty(requestBody)
                ? "This HTTP triggered function executed successfully. Data saved and QR code generated."
                : string.Empty;
            //return new OkObjectResult(responseMessage);
            return new FileContentResult(resultQR, "image/png");

        }
    }
}
