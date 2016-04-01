using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSEP.Common;
using Security.SecureKeys;
using SignalR.Common;
using BSEP.Business;

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
                    Convert.FromBase64String(Request.ReadHeaderValue(Constants.HeaderKeys.Certificate));
				Identity.ChatClientName = Request.ReadHeaderValue(Constants.HeaderKeys.SenderIdenttiy);
				KeysManager.AddPublicKeyToList(Identity.ChatClientName, clientCertificate);

				//prepare response whit my key
				response.Headers.Add(Constants.HeaderKeys.Certificate, Convert.ToBase64String(KeysManager.GetMyCertificateByteArray()));
				response.Headers.Add(Constants.HeaderKeys.SenderIdenttiy, Identity.UserName);
			}
			catch (Exception ex)
			{
				
			}
			return response;
		}
	}
}