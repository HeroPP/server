using System.Runtime.Serialization;

namespace Hero.Server.Core.Exceptions
{

    [Serializable]
    public class BaseException : Exception
    {
        public BaseException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public BaseException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public BaseException(int errorCode, string message, Exception inner) : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public int ErrorCode { get; set; }
    }
}
