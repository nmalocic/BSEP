using System;
using System.Threading;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
//[assembly: OwinStartup(typeof(SignalR.Startup))]
namespace SignalR
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// Any connection or hub wire up and configuration should go here
			app.MapSignalR();

			// Configure Web API for self-host. 
			HttpConfiguration config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			app.UseWebApi(config);

			//Thread thread = new Thread(() =>
			//{
			//	while (true)
			//	{
			//		var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
			//		context.Clients.All.time(DateTime.Now.ToLongTimeString());

			//		System.Threading.Thread.Sleep(1000);
			//	}
			//});
			//thread.Start();
		}
	}
}