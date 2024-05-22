using Microsoft.AspNetCore.Http;
using Solution.DAL.Data;
using Solution.DAL.Models;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Solution.Business.Utility
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, AppDbContext context)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex, context);
			}
		}
		private static async Task HandleExceptionAsync(HttpContext context, Exception exception, AppDbContext dbContext)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var pathSegments = context.Request.Path.Value?.Trim('/').Split('/');
			var actionName = pathSegments?.Length > 2 ? pathSegments[2] : "Unknown";
			
					var exceptionLog = new ExceptionLog
					{
						ControllerName = context.Request.Path,
						ActionName = actionName, 
						ExceptionMessage = exception.Message + " "+ exception.InnerException,
						StackTrace = exception.StackTrace,
						Timestamp = DateTime.Now 
					};
					dbContext.ChangeTracker.Clear();
					dbContext.ExceptionLogs.Add(exceptionLog);
					await dbContext.SaveChangesAsync();
				
			var response = new { error = exception.Message + "/t Inner Exception " + exception.InnerException};
			var jsonResponse = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(jsonResponse);
		}
	}
}
