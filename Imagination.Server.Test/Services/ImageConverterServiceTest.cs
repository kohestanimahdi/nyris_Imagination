using Imagination.Exceptions;
using Imagination.Services.ImageConverter;

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

        [Fact]
        public void GetImageAsJPEGFormat_ErrorInvalidType()
        {
            var memoryStream = new MemoryStream();
            using var fs = File.OpenRead(Path.Combine("Resources", "Mahdi_Kouhestani_Resume.pdf"));
            fs.CopyTo(memoryStream);

            var exception = Assert.Throws<ImageConverterException>(() => _imageConverterService.GetImageAsJPEGFormat(memoryStream));

            Assert.Equal(ImageConverterExceptionType.InvalidFile, exception.Type);
        }

        [Fact]
        public void GetImageAsJPEGFormat_ConvertPNG()
        {
            var memoryStream = new MemoryStream();
            using var fs = File.OpenRead(Path.Combine("Resources", "png.png"));
            fs.CopyTo(memoryStream);

            var result = _imageConverterService.GetImageAsJPEGFormat(memoryStream);

            var mimeType = ImageConverterService.GetMimeTypeFromImageByteArray(result);

            Assert.NotEmpty(result);
            Assert.Equal(System.Net.Mime.MediaTypeNames.Image.Jpeg, mimeType);
        }

        [Fact]
        public void GetImageAsJPEGFormat_ReturnExactJPEG()
        {
            var memoryStream = new MemoryStream();
            using var fs = File.OpenRead(Path.Combine("Resources", "big.jpg"));
            fs.CopyTo(memoryStream);

            var result = _imageConverterService.GetImageAsJPEGFormat(memoryStream);
            var expectedByteArray = memoryStream.ToArray();
            var mimeType = ImageConverterService.GetMimeTypeFromImageByteArray(result);

            Assert.NotEmpty(result);
            Assert.Equal(expectedByteArray, result);
            Assert.Equal(System.Net.Mime.MediaTypeNames.Image.Jpeg, mimeType);
        }
    }
}
