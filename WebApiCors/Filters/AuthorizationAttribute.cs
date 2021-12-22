using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiCors.Helpers;

namespace WebApiCors.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IJwtService _jwtService;

        public AuthorizationAttribute(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwt = context.HttpContext.Request.Cookies["jwt"];           
            var token = _jwtService.Verify(jwt);           
            if(token is null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
