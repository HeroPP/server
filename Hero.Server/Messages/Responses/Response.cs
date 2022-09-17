namespace Hero.Server.Messages.Responses
{
    public class Response<T>
    {
        public bool Succeeded { get; set; } = false;
        public T Data { get; set; }
        public List<ErrorResponse> Errors { get; private set; } = new();

        public void AddError(int code, string message)
        {
            this.Errors.Add(new(code, message));
        }
    }

    public class Response
    {
        public bool Succeeded { get; set; } = false;
        public List<ErrorResponse> Errors { get; private set; } = new();

        public void AddError(int code, string message)
        {
            this.Errors.Add(new(code, message));
        }
    }
}
