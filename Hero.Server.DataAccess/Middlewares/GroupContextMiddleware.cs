using Hero.Server.Core.Database;

using Microsoft.AspNetCore.Http;

namespace Hero.Server.DataAccess.Middlewares
{
    public class GroupContextMiddleware
    {
        private readonly RequestDelegate next;

        public GroupContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IGroupContextBuilder group)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                if (context.Request.Headers.ContainsKey("Group"))
                {
                    if (Guid.TryParse(context.Request.Headers["Group"], out Guid groupId)) {
                        group.Apply(groupId);
                    }
                }
            }

            await this.next(context);
        }
    }
}
