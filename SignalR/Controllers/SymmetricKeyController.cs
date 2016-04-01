using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSEP.Common;
using Security.SecureKeys;
using BSEP.Business;

namespace SignalR
{
	public class SymmetricKeyController : ApiController
	{
		// GET api/<controller>
		public HttpResponseMessage Post([FromBody]string value)
		{
			var clientIdentity = Request.Headers.GetValues(Constants.HeaderKeys.SenderIdenttiy).FirstOrDefault();
			var response = Request.CreateResponse(HttpStatusCode.OK);
			var keyAndIv = KeysManager.GenerateSymetricKey(clientIdentity);

			response.Headers.Add(Constants.HeaderKeys.SenderIdenttiy, Identity.UserName);
			response.Headers.Add(Constants.HeaderKeys.SymmetricKey, keyAndIv.Item1);
			response.Headers.Add(Constants.HeaderKeys.SymmetricIv, keyAndIv.Item2);

			return response;
		}
	}
}