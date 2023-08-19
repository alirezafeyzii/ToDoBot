using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace ToDoBot.Infrastructure.Extensions
{
	public static class WebHookExtension
	{
		public static T GetConfiguration<T>(this IServiceProvider serviseProvider) where T : class
		{
			var o = serviseProvider.GetService<IOptions<T>>();

			if (o is null)
				throw new ArgumentNullException(nameof(o));

			return o.Value;
		}

		public static ControllerActionEndpointConventionBuilder MapWebhookRoute<T>(
			this IEndpointRouteBuilder endpoints,
			string route)
		{
			var controllerName = typeof(T).Name.Replace("Controller", "", StringComparison.Ordinal);
			var actionName = typeof(T).GetMethods()[0].Name;

			return endpoints.MapControllerRoute(
				name: "bot_webhook",
				pattern: route,
				defaults: new { controller = controllerName, action = actionName });
		}
	}
}
