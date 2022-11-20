using System.Runtime.Serialization;

namespace Hero.Server.Core.Exceptions
{

    [Serializable]
    public class BaseException : Exception
    {
        public BaseException(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public BaseException(ErrorCode errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public BaseException(ErrorCode errorCode, string message, Exception inner) : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ErrorCode ErrorCode { get; set; }
    }
}
