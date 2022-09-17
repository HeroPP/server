using Microsoft.Extensions.Logging;

namespace Hero.Server.Core.Logging
{
    public static partial class ILoggerExtension
    {
        [LoggerMessage((int)EventIds.UnknownErrorOccured, LogLevel.Error, "An unknown error occured.")]
        public static partial void LogUnknownErrorOccured(this ILogger logger, Exception ex);

        [LoggerMessage((int)EventIds.UserAlreadyExists, LogLevel.Error, "A user with this id already exists")]
        public static partial void LogUserAlreadyExists(this ILogger logger);

        [LoggerMessage((int)EventIds.CharacterDoesNotExist, LogLevel.Error, "The character (id: {CharacterId}) you're trying to delete does not exist.")]
        public static partial void LogCharacterDoesNotExist(this ILogger logger, Guid characterId);
    }
}
