namespace Hero.Server.Core
{
    public class GlobalAttribute
    {
        public static readonly Models.Attribute Health = new()
        {
            Id = new Guid("c158788d-075d-4456-88a5-88882c845102"),
            Name = "Health",
            Description = "The health state of the character.",
            GroupId = new Guid(),
            IconData = "{\"pack\":\"fontAwesomeIcons\",\"key\":\"heartPulse\"}",
            MinValue = 0,
            MaxValue = Int32.MaxValue,
            StepSize = 5,
        };
    }
}
