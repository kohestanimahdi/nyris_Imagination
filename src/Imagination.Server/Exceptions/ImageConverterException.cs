using System;

namespace Imagination.Exceptions
{
    public enum ImageConverterExceptionType
    {
        Default,
        EmptyStream,
        InvalidFile
    }
    public class ImageConverterException : Exception
    {

        public ImageConverterException(ImageConverterExceptionType type, Exception innerEception, string message = null) : base(message, innerEception)
        {

        }

        public ImageConverterException(ImageConverterExceptionType type, string message = null) : base(message)
        {
        }
    }
}
