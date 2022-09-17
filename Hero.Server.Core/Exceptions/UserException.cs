using System.Runtime.Serialization;

namespace Hero.Server.Core.Exceptions
{

    [Serializable]
    public class UserException : BaseException
    {
        public UserException(int errorCode) : base(errorCode)
        { }

        public UserException(int errorCode, string message) : base(errorCode, message)
        { }

        public UserException(int errorCode, string message, Exception inner) : base(errorCode, message, inner)
        { }

        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
