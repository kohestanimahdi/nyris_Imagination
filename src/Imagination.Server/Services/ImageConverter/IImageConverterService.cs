using Microsoft.AspNetCore.Http;
using System.IO;

namespace Imagination.Services.ImageConverter
{
    public interface IImageConverterService
    {
        byte[] GetImageAsJPEGFormat(MemoryStream memoryStream);
        byte[] GetImageAsJPEGFormat(IFormFile formFile);
    }
}