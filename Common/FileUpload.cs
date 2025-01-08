using Azure.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Stock_CMS.Common
{

    public class Upload
    {
        public bool? status;
        public string? message;
    }
    public class FileUpload
    {
        private readonly IConfiguration _configuration;

        public FileUpload(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Upload StoreFile(string fileType, IFormFile? file)
        {
            var result = new Upload
            {
                status = false,
                message = "Error"
            };
            var directoryPath = _configuration.GetSection("UploadPath")?.GetValue<string>($"{fileType}Path");
            
            var path = _configuration.GetSection("UploadPath")?.GetValue<string>("DomainName");

            try
            {
                if (directoryPath != null && file != null )
                {

                    var up = StoreFileToPath(directoryPath, file);

                    result = new Upload
                    {
                        status = true,
                        message = (path + up).Replace("wwwroot\\", "/").Replace("\\", "/"),
                    };

                  
                }
                return result;

            }
            catch (Exception ex)
            {
                result = new Upload
                {
                    status = false,
                    message = ex.Message,
                };
                return result;
            }
        }


        public string StoreFileToPath(string directoryPath, IFormFile? file)
        {
            var filePath = string.Empty;


            try
            {
                if (directoryPath != null && file != null)
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    filePath = Path.Combine(directoryPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                }
                return filePath;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }
    }

}
