using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Solution.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuthenticationMiddleware
{
	private readonly RequestDelegate _next;
	private readonly JwtSettings _jwtSettings;

	public AuthenticationMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
	{
		_next = next;
		_jwtSettings = jwtSettings.Value;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var pathsWithoutAuthentication = new[] { "/api/account/login", "/api/account/register" };

		if (!RequiresAuthentication(context.Request.Path, pathsWithoutAuthentication))
		{
			await _next(context);
			return;
		}

		if (!IsAuthenticated(context))
		{
            if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync("Unauthorized. Please authenticate.");
                return;
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized access. Please log in.");
                return;
            }
        }

		await _next(context);
	}

	private bool RequiresAuthentication(PathString path, string[] pathsWithoutAuthentication)
	{
		return !pathsWithoutAuthentication.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
	}
	private bool IsAuthenticated(HttpContext context)
	{
		var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

		if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		var token = authorizationHeader.Substring("Bearer ".Length).Trim();
		return TokenIsValid(token);
	}
	private bool TokenIsValid(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var validationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
			ValidateIssuer = true,
			ValidIssuer = _jwtSettings.ValidIssuer,
			ValidateAudience = true,
			ValidAudience = _jwtSettings.ValidAudience,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};

		try
		{
			tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
			return validatedToken is JwtSecurityToken jwtToken &&
				   jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
		}
		catch
		{
			return false;
		}
	}
}
