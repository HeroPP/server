namespace Hero.Server.Messages.Responses
{
    public class CreateCharacterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
