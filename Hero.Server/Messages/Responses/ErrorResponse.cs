namespace Hero.Server.Messages.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}