using Imagination.Services.ImageConverter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagination.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageConverterController : ControllerBase
    {
        private readonly IImageConverterService _imageConverter;

        public ImageConverterController(IImageConverterService imageConverter)
        {
            _imageConverter = imageConverter ?? throw new ArgumentNullException(nameof(imageConverter));
        }


        /// <summary>
        /// Convert the image file that is get from the stream body to JPEG format
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/convert")]
        public async Task<IActionResult> ConvertImageToJPEGAsync(CancellationToken cancellationToken)
        {
            byte[] resultFile = null;
            // the image may post as a form file, so we have to consider both
            if (Request.HasFormContentType && Request.Form?.Files is { Count: > 0 })
            {
                // consider the first file of the form
                resultFile = _imageConverter.GetImageAsJPEGFormat(Request.Form.Files.First());
            }
            else
            {
                using var memoryStream = new MemoryStream();

                // if the data is sended in stream, we have to read all the stream
                await Request.BodyReader.CopyToAsync(memoryStream, cancellationToken);

                resultFile = _imageConverter.GetImageAsJPEGFormat(memoryStream);
            }

            // return the byte array of the file to the cliend with content header image/jpeg
            return File(resultFile, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        }
    }
}
