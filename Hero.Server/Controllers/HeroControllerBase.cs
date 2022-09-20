using Hero.Server.Core;
using Hero.Server.Core.Exceptions;
using Hero.Server.Core.Logging;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    public class HeroControllerBase : ControllerBase
    {
        protected readonly ILogger logger;

        public HeroControllerBase(ILogger logger)
        {
            this.logger = logger;
        }

        protected IActionResult HandleExceptions(Func<IActionResult> action)
        {
            try
            {
                return action.Invoke();
            }
            catch(BaseException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                return this.BadRequest(new ErrorResponse(ex.ErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                return this.BadRequest(new ErrorResponse((int)EventIds.UnknownErrorOccured, "An unknown error occured, while processing your request."));
            }
        }

        protected async Task<IActionResult> HandleExceptions(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action.Invoke();
            }
            catch (BaseException ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                return this.BadRequest(new ErrorResponse(ex.ErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                this.logger.LogUnknownErrorOccured(ex);
                return this.BadRequest(new ErrorResponse((int)EventIds.UnknownErrorOccured, "An unknown error occured, while processing your request."));
            }
        }
    }
}
