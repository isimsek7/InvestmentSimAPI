using System;
namespace CryptoAnalyzer.WebApi.Middlewares
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder app)
		{
			return app.UseMiddleware<MaintenanceMiddleware>();
		}

        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }


    }
}

