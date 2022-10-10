using CarsApi.Validations;
using Microsoft.AspNetCore.Http;
using System;

namespace CarsApi.services
{
    public class LocalFileSaver : ILocalFileSaver
    {
        private readonly IWebHostEnvironment environment;
        public LocalFileSaver(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task RemoveAsync(string route, FileEnumType fileEnumType)
        {
            if (fileEnumType == FileEnumType.Image)
            {
                await deleteImg(route);
                return;
            }
            throw new NotImplementedException();
        }
        async Task deleteImg(string fileRoute)
        {
            await Task.Run(() =>
            {
                var baseRoute = $"{environment.WebRootPath}\\images\\";
                fileRoute = fileRoute.Remove(0, 13);
                var fileAbsoluteRoute = baseRoute + fileRoute;
                System.IO.File.Delete(fileAbsoluteRoute);
            });
            
        }
        public async Task<string> SaveAsync(IFormFile formFile, FileEnumType fileEnumType)
        {
            if (fileEnumType == FileEnumType.Image)
            {
                return await saveImg(formFile);
            }
            throw new NotImplementedException();
        }
        async Task<string> saveImg(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }
            if (formFile.Length <= 0)
            {
                return null;
            }
            var fileExtension = formFile.ContentType.Split("/").LastOrDefault();
            if (fileExtension == null)
            {
                return null;
            }
            var baseRoute = $"{environment.WebRootPath}\\images\\";
            if (!Directory.Exists(baseRoute))
            {
                Directory.CreateDirectory(baseRoute);
            }
            var fileRoute = DateTime.Now.ToString("yyMMddHHmmssff") + "." + fileExtension;
            var fileAbsoluteRoute = baseRoute + fileRoute;
            using (FileStream fileStream = System.IO.File.Create(fileAbsoluteRoute))
            {
                await formFile.CopyToAsync(fileStream);
                fileStream.Flush();
                return "/media/images/" + fileRoute;
            }
        }
    }
}
