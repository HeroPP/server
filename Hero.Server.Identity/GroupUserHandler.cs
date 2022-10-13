using Microsoft.AspNetCore.Authorization;

namespace Hero.Server.Identity
{
    public class GroupUserHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
