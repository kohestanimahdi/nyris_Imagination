using System.IO;

namespace Imagination.Services.ImageConverter
{
    public interface IImageConverterService
    {
        byte[] GetImageAsJPEGFormat(MemoryStream memoryStream);
    }
}