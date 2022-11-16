namespace Hero.Server.Core
{
    public enum ErrorCode
    {
        UnkownError = 0x0,

        // Group Error: 10XX

        UnknownGroupError = 0x1000,
        InvalidCode = 0x1001,
        NotGroupAdmin = 0x1002,
        GroupAlreadyExist = 0x1003,
        GroupOwnerNotFound = 0x1004,
        CouldNotJoinGroup = 0x1005,
        GroupNotFound = 0x1006,

        //Ability Error: 11XX

        UnknownAbilityError = 0x1100,
        AbilityNotFound = 0x1101
    }
}
