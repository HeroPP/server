using Hero.Server.Core;
using Hero.Server.Core.Exceptions;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    public class HeroControllerBase : ControllerBase
    {
        protected async Task<Response> HandleExceptions(Func<Task> action)
        {
            Response response = new();
            try
            {
                await action();
                response.Succeeded = true;
            }
            catch (BaseException ex)
            {
                response.AddError(ex.ErrorCode, ex.Message);
            }
            catch (Exception)
            {
                response.AddError((int)EventIds.UnknownErrorOccured, "An unknown error occured, please try again later");
            }

            return response;
        }

        protected async Task<Response<T>> HandleExceptions<T>(Func<Task<T>> action)
        {
            Response<T> response = new();
            try
            {
                T result = await action();
                response.Data = result;
                response.Succeeded = true;
            }
            catch (BaseException ex)
            {
                response.AddError(ex.ErrorCode, ex.Message);
            }
            catch (Exception)
            {
                response.AddError((int)EventIds.UnknownErrorOccured, "An unknown error occured, please try again later");
            }

            return response;
        }
    }
}
