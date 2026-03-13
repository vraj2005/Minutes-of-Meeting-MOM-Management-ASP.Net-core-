using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MOM_Project.Infrastructure
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class CheckAccessAttribute : ActionFilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var userId = context.HttpContext.Session.GetString("UserID");
			if (string.IsNullOrWhiteSpace(userId))
			{
				context.Result = new RedirectResult("~/User/Login");
			}
		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
			context.HttpContext.Response.Headers["Expires"] = "-1";
			context.HttpContext.Response.Headers["Pragma"] = "no-cache";
			base.OnResultExecuting(context);
		}
	}
}
