﻿using System.Security.Claims;

namespace SchoolManagementSystem.API.Extensions
{
	public static class UserExtensions
	{
		public static string? GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}