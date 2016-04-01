using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Xml;
using BSEP.Common;
using Microsoft.AspNet.SignalR;
using Security;
using Security.SecureKeys;
using SignalR.Common;
using BSEP.Business;

namespace SignalR
{
    public class ChatController : ApiController
	{
		// POST api/<controller>
		public HttpResponseMessage Post([FromBody]string value)
		{
            string name, message;
            MessageGenerator.ReadXmlMessage(Request.ReadHeaderValue(Constants.HeaderKeys.Message), out message, out name);

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.broadcastMessageValue(name, message);

			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}
	}
}