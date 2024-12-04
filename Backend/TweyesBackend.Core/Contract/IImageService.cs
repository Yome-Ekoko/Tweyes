using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.Contract
{
    public interface IImageService
    {
        Task<string> UploadSingle(IFormFile file, string folder);
    }
}
