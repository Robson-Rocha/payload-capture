using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PayloadCapture
{
    public class Capture
    {
        private readonly string TargetBlobContainer;
        private readonly StorageAccountAttribute StorageAccountAttribute;

        public Capture()
        {
            TargetBlobContainer = Environment.GetEnvironmentVariable("TargetBlobContainer");
            StorageAccountAttribute = new StorageAccountAttribute("TargetStorageAccount");
        }

        [FunctionName("Capture")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            Binder binder,
            ILogger log)
        {
            // Try (not too hard) to guess the output type extension based on the request's ContentType header
            string extension;
            if (req.ContentType.Contains("json"))
            {
                extension = "json";
            }
            else if (req.ContentType.Contains("xml"))
            {
                extension = "xml";
            }
            else
            {
                extension = "txt";
            }
            
            // Sets the output path inside Azure Storage Blob based on the Target Blob Container, the current date, and the guessed extension
            string blobPath = $"{TargetBlobContainer ?? "captured-payloads"}/{DateTime.Now:ddMMyyyyhhmmssffff}_{Guid.NewGuid()}";

            // Dinamically binds the Blob output binding and copies the request payload to it
            var payloadOutputBindingAttributes = new Attribute[]
            {
                StorageAccountAttribute,
                new BlobAttribute($"{blobPath}__payload.{extension}", FileAccess.Write),
            };
            using (var capturedPayload = await binder.BindAsync<Stream>(payloadOutputBindingAttributes))
            {
                await req.Body.CopyToAsync(capturedPayload);
            }

            // Dinamically binds the Blob output binding and copies the request payload to it
            var headersOutputBindingAttributes = new Attribute[]
            {
                StorageAccountAttribute,
                new BlobAttribute($"{blobPath}__headers.txt", FileAccess.Write),
            };
            using (var headersWriter = await binder.BindAsync<TextWriter>(headersOutputBindingAttributes))
            {
                foreach (var header in req.Headers)
                {
                    headersWriter.WriteLine($"{header.Key}: {header.Value}");
                }
            }

            return new OkResult();
        }
    }
}
