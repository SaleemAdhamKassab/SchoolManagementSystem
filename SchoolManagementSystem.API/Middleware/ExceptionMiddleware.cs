using SchoolManagementSystem.Application.Contracts.Common;
using System.Net;
using System.Text.Json;

namespace SWR.Api.Middleware
{
	public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
	{
		private readonly RequestDelegate _next = next;
		private readonly ILogger<ExceptionMiddleware> _logger = logger;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				// Serilog will automatically capture the exception details as a structured object.
				_logger.LogError(ex, "Unhandled exception occurred");

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var response = new GeneralResponse<object>(false, ex.Message, null, StatusCodes.Status500InternalServerError);

				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
		}
	}
}