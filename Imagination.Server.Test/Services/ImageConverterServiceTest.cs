using Imagination.Exceptions;
using Imagination.Services.ImageConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Server.Test.Services
{
    public class ImageConverterServiceTest
    {
        private readonly IImageConverterService _imageConverterService;
        public ImageConverterServiceTest()
        {
            _imageConverterService = new ImageConverterService();
        }

        [Fact]
        public void GetImageAsJPEGFormat_ErrorStreamIsNull()
        {
            MemoryStream memoryStream = null;
            var exception = Assert.Throws<ImageConverterException>(() => _imageConverterService.GetImageAsJPEGFormat(memoryStream));

            Assert.Equal(ImageConverterExceptionType.EmptyStream, exception.Type);
        }

        [Fact]
        public void GetImageAsJPEGFormat_ErrorStreamIsEmpty()
        {
            MemoryStream memoryStream = new MemoryStream();
            var exception = Assert.Throws<ImageConverterException>(() => _imageConverterService.GetImageAsJPEGFormat(memoryStream));

            Assert.Equal(ImageConverterExceptionType.EmptyStream, exception.Type);
        }
    }
}
