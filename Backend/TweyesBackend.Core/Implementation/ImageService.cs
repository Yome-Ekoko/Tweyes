using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweyesBackend.Core.Contract;
using TweyesBackend.Domain.Settings;

namespace TweyesBackend.Core.Implementation
{
    public class ImageService : IImageService
    {
        private readonly AzureStorageOptions _azureStorageOptions;

        public ImageService(IOptions<AzureStorageOptions> azureStorageOptions)
        {
            _azureStorageOptions = azureStorageOptions.Value;
        }
        public async Task<string> UploadSingle(IFormFile file, string folder)
        {
            if (file == null)
                return "No files received from the upload";

            BlobServiceClient blobServiceClient = new BlobServiceClient(_azureStorageOptions.ConnectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_azureStorageOptions.ContainerName);
            containerClient.CreateIfNotExists();

            decimal fileSizeLimit = 200 * 1024 * 1024;

            string blobName = "";
            //BlobClient blobClient;
            //string blobUrl = "";

            if (IsImage(file))
            {
                if (file.Length > 0 && file.Length <= fileSizeLimit)
                {
                    blobName = SanitizeBlobName(file.FileName);

                    using (Stream stream = file.OpenReadStream())
                    {
                        var upl = await containerClient.UploadBlobAsync($"users/{folder}/{blobName}", stream);
                    }


                }
                else
                {
                    throw new Exception("Please upload a video less than 200mb");
                }
            }
            else
            {
                return "Unsupported media type selected";
            }

            Uri blobUri = new Uri("https://" +
                                 _azureStorageOptions.StorageAccountName +
                                 ".blob.core.windows.net/" +
                                 _azureStorageOptions.ContainerName +
                                 "/" + $"users/{folder}/{blobName}");

            //blobClient = containerClient.GetBlobClient($"users/{blobName}");

            //blobUrl = blobClient.Uri.ToString();

            return blobUri.ToString();
        }

        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".mp4", ".3gp", ".pdf" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
        private string SanitizeBlobName(string originalBlobName)
        {
            char[] invalidChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '#', '{', '}', '%', '&' };

            foreach (var invalidChar in invalidChars)
            {
                originalBlobName = originalBlobName.Replace(invalidChar, '_');
            }

            if (originalBlobName.Length > 255)
            {
                originalBlobName = originalBlobName.Substring(0, 255);
            }

            return originalBlobName;
        }
    }
}

