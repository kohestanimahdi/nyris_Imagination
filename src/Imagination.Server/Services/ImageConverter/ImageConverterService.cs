using Imagination.Exceptions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Services.ImageConverter
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class ImageConverterService : IImageConverterService
    {
        public byte[] GetImageAsJPEGFormat(MemoryStream memoryStream)
        {
            ValidateMemoryStream(memoryStream);

            var mimeType = GetMimeTypeFromImageMemoryStream(memoryStream);
            byte[] resultFile;

            if (mimeType == System.Net.Mime.MediaTypeNames.Image.Jpeg)
                resultFile = memoryStream.ToArray();
            else
            {
                resultFile = ConvertImageToJPEG(memoryStream);
            }

            return resultFile;
        }

        private byte[] ConvertImageToJPEG(MemoryStream memoryStream)
        {
            using var image = new Bitmap(memoryStream);

            using var newImageStream = new MemoryStream();

            image.Save(newImageStream, ImageFormat.Jpeg);

            return newImageStream.ToArray();
        }

        private static string GetMimeTypeFromImageMemoryStream(MemoryStream memoryStream)
        {
            using var image = Image.FromStream(memoryStream, true, false);

            var imageEncoder = ImageCodecInfo.GetImageEncoders()
                                              .FirstOrDefault(codec => codec.FormatID == image.RawFormat.Guid);

            if (imageEncoder is null)
                throw new ImageConverterException(ImageConverterExceptionType.InvalidFile);

            return imageEncoder.MimeType;
        }

        private static void ValidateMemoryStream(MemoryStream memoryStream)
        {
            if (memoryStream is null || memoryStream.Length == 0)
                throw new ArgumentNullException(nameof(memoryStream), "Stream data is null or empty.");
        }
    }
#pragma warning restore CA1416 // Validate platform compatibility
}
