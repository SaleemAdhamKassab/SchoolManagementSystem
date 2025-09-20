namespace SchoolManagementSystem.Application.Contracts.Common
{
	public class GeneralResponse<T>(bool success, string? message, T? data, int statusCode)
	{
		public bool Success { get; set; } = success;
		public string? Message { get; set; } = message;
		public T? Data { get; set; } = data;
		public int StatusCode { get; set; } = statusCode;
	}
}