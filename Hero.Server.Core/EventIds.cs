namespace Hero.Server.Core
{
    public enum EventIds
    {
        // Common events: 0XXX
        UnknownErrorOccured = 0000,

        // User Events: 1XXX
        UnknownUserError = 1000,
        UserAlreadyExists = 1001,
        UserCreated = 1002,

        // Character Events: 2XXX
        CharacterDoesNotExist = 2001,
    }
}
