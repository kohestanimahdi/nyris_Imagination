using Imagination.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Imagination.Services.ImageConverter
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class ImageConverterService : IImageConverterService
    {
        public byte[] GetImageAsJPEGFormat(IFormFile formFile)
        {
            //if the data is sended in form file, it is converted to the stream and then call the convert function which is get the stream as input
            using var ms = new MemoryStream();

            formFile.CopyTo(ms);

            return GetImageAsJPEGFormat(ms);
        }

        public byte[] GetImageAsJPEGFormat(MemoryStream memoryStream)
        {
            // validate the stream is not null and also has length
            ValidateMemoryStream(memoryStream);

            // get the mime type of the file
            var mimeType = GetMimeTypeFromImageMemoryStream(memoryStream);
            byte[] resultFile;

            // if the file type equals to JPEG, it does not need to reformat
            if (mimeType == System.Net.Mime.MediaTypeNames.Image.Jpeg)
                resultFile = memoryStream.ToArray();
            else
                resultFile = ConvertImageToJPEG(memoryStream); // get converted file as byte array


            return resultFile;
        }

        private byte[] ConvertImageToJPEG(MemoryStream memoryStream)
        {
            // read the stream as bitmap
            using var image = new Bitmap(memoryStream);

            using var newImageStream = new MemoryStream();

            // save the bit map into another mempory stream with JPEG format
            image.Save(newImageStream, ImageFormat.Jpeg);

            // return the streamed data as byte array
            return newImageStream.ToArray();
        }

        private static string GetMimeTypeFromImageMemoryStream(MemoryStream memoryStream)
        {
            try
            {
                // try to read the fime as a image, if the file is not image, it throws an exception
                using var image = Image.FromStream(memoryStream, true, false);

                // find th image code info of the file to reconize the mime type
                var imageEncoder = ImageCodecInfo.GetImageEncoders()
                                                  .FirstOrDefault(codec => codec.FormatID == image.RawFormat.Guid);

                return imageEncoder.MimeType;
            }
            catch (Exception ex)
            {
                throw new ImageConverterException(ImageConverterExceptionType.InvalidFile, innerEception: ex);
            }

        }

        private static void ValidateMemoryStream(MemoryStream memoryStream)
        {
            if (memoryStream is null || memoryStream.Length == 0)
                throw new ImageConverterException(ImageConverterExceptionType.EmptyStream);
        }
    }
#pragma warning restore CA1416 // Validate platform compatibility
}
