﻿namespace Hero.Server.Core
{
    public enum EventIds
    {
        // Common events: 0XXX
        UnknownErrorOccured = 0000,

        // Model Events 1XXX

        // User Events: 11XX
        UnknownUserError = 1100,
        UserAlreadyExists = 1101,
        UserCreated = 1102,
        // Character Events: 12XX
        CharacterDoesNotExist = 1201,

        // Ability Events: 13XX
        AbilityDoesNotExist = 1301,

        // Node Events: 14XX
        NodeDoesNotExist = 1401,

        // Skill Events: 15XX
        SkillDoesNotExist = 1501,

        // NodeTree Events: 16XX
        NodeTreeDoesNotExist = 1601,

        // Group Events: 17XX
        GroupCreationFailed = 1700,
        GroupCreated = 1701,
        InvalidInvitationCode = 1702,
        CannotCreateCharacterOutsideOfAGroup = 1703,
        NotAGroupAdmin = 1704,

        // Attribute Events 18XX
        AttributeDoesNotExist = 1800,

        // Race Events 19XX
        RaceDoesNotExist = 1900, //this is not racist.
    }
}
