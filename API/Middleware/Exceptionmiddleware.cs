using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionMiddleware
{
	private readonly IHostEnvironment _env;
	private readonly RequestDelegate _next;

	public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
	{
		_next = next;
		_env = env;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception ex)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var response = _env.IsDevelopment()
			? new ApiErrorResponse(
				context.Response.StatusCode,
				ex.Message,
				ex.StackTrace)
			: new ApiErrorResponse(
				context.Response.StatusCode,
				"Internal server error");

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		var json = JsonSerializer.Serialize(response, options);
		return context.Response.WriteAsync(json);
	}
}
