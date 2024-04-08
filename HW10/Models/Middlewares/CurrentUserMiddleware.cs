using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HW10.Models.Middlewares
{
	public class CurrentUserMiddleware
	{
		private readonly RequestDelegate _next;
		public CurrentUserMiddleware(RequestDelegate next) 
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, SiteDbContext dbContext)
		{
			var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
			if (context.User.Identity.IsAuthenticated)
			{
				context.Items["CurrentUser"] = await dbContext
					.Users
					.Include(x => x.Image)
					.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
			}
			
			await _next(context);
		}
	}
}
