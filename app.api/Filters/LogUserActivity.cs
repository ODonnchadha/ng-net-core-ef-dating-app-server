using app.api.Interfaces.Respositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace app.api.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var repository = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            var user = await repository.GetUser(
                int.Parse(resultContext.HttpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier).Value));

            user.LastActive = DateTime.Now;

            await repository.SaveAll();
        }
    }
}
