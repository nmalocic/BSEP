using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Xml;
using BSEP.Common;
using Microsoft.AspNet.SignalR;
using Security;
using Security.SecureKeys;
using SignalR.Common;

namespace SignalR
{
	public class ChatController : ApiController
	{
		// POST api/<controller>
		public HttpResponseMessage Post([FromBody]string value)
		{
			var host = Request.RequestUri.Host;
			var encodedValue = Request.ReadHeaderValue(Constants.HeaderKeys.Message);

			var xml = Uri.UnescapeDataString(encodedValue);
			var clientCertificate = KeysManager.GetClientKey(Identity.ChatClientName);
			var keyToDecrypt = (RSACryptoServiceProvider) clientCertificate.PublicKey.Key;
			var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
			context.Clients.All.publicTokenAquired(StringBuilderUtility.GetKeyUsedToDecryptInfoHtml(keyToDecrypt));

			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);
			
			if (!SignatureProvider.VerifyXml(document, keyToDecrypt))
				return Request.CreateResponse(HttpStatusCode.ExpectationFailed);

			var symmetricKey = KeysManager.GetCurrentSymetricKeyForClient(Identity.ChatClientName);
			EncryptionProvider.Decrypt(document, symmetricKey);

			var message = document.GetElementsByTagName(Constants.ElementNames.Message)[0].InnerText.Trim();
			var name = document.GetElementsByTagName(Constants.ElementNames.Sender)[0].InnerText.Trim();

			context.Clients.All.broadcastMessageValue(name, message);

			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}
	}
}