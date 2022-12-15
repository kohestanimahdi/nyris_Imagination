using System;

namespace Imagination.Exceptions
{
    public class ImageConverterException : Exception
    {
        public enum ImageConverterExceptionType : string
        {
            a = "a",

        }
        public ImageConverterException(string message) : base(message)
        {
        }
    }
}
