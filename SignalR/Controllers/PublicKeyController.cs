using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using BSEP.Common;
using Security.SecureKeys;

namespace SignalR
{
	public class PublicKeyController : ApiController
	{
		// POST api/<controller>
		public HttpResponseMessage Post([FromBody]string value)
		{
			var response = Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				//Read public key from headers and add to list
				var clientCertificate =
					Convert.FromBase64String(Request.Headers.GetValues(Constants.HeaderKeys.Certificate).FirstOrDefault());
				var clientIdentity = Request.Headers.GetValues(Constants.HeaderKeys.SenderIdenttiy).FirstOrDefault();
				KeysManager.AddPublicKeyToList(clientIdentity, new X509Certificate2(clientCertificate));

				//prepare response whit my key
				var myPublicKey = KeysManager.GetMyCertificate();
				response.Headers.Add(Constants.HeaderKeys.Certificate, Convert.ToBase64String(myPublicKey.RawData));
				response.Headers.Add(Constants.HeaderKeys.SenderIdenttiy, Identity.UserName);
			}
			catch (Exception ex)
			{
				
			}
			return response;
		}
	}
}