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
        public ImageConverterExceptionType Type { get; }

        public ImageConverterException(ImageConverterExceptionType type, Exception innerEception, string message = null) : base(message, innerEception)
        {
            Type = type;
        }

        public ImageConverterException(ImageConverterExceptionType type, string message = null) : base(message)
        {
            Type = type;
        }

        public string GetErrorTypeDescription()
            => Type switch
            {
                ImageConverterExceptionType.EmptyStream => "File is null or Empty",
                ImageConverterExceptionType.InvalidFile => "File format is not valud",
                _ => "UnHandled Exception"
            };
    }
}
