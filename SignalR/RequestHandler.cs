using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using BSEP.Business;
using BSEP.Common;
using Security;
using Security.SecureKeys;
using SignalR.Common;

namespace SignalR
{
	public class RequestHandler
	{
		public void ProcessSendMessageRequest(string sender, string message)
		{
			var url = BuildUrl(Constants.Endpoints.Chat);
			var request = WebUtils.PrepareRequestWithMessage(url, MessageGenerator.GenerateXmlMesasgeForClient(sender, message, Identity.ChatClientName));

			using (var response = request.GetResponse())
			{
			}
		}

		public void ExchangeCertificates()
		{
			var url = BuildUrl(Constants.Endpoints.ExchangeCertificate);
			var request = WebUtils.PrepareWebRequestWithCertificate(url, KeysManager.GetMyCertificate(), Identity.UserName);
			using (var response = request.GetResponse())
			{
				var certificate = Convert.FromBase64String(response.ReadHeaderValue(Constants.HeaderKeys.Certificate));
				Identity.ChatClientName = response.ReadHeaderValue(Constants.HeaderKeys.SenderIdenttiy);
				KeysManager.AddPublicKeyToList(Identity.ChatClientName, new X509Certificate2(certificate));
			}
		}

		public void ExchangeSymetricKey()
		{
			var url = BuildUrl(Constants.HeaderKeys.SymmetricKey);
			var request = WebUtils.PrepareWebRequest(url);
			request.Headers.Add(Constants.HeaderKeys.SenderIdenttiy, Identity.UserName);

			using (var response = request.GetResponse())
			{
				var clientIdentity = response.ReadHeaderValue(Constants.HeaderKeys.SenderIdenttiy);
				var key = response.ReadHeaderValue(Constants.HeaderKeys.SymmetricKey);
				var iv = response.ReadHeaderValue(Constants.HeaderKeys.SymmetricIv);

				var bytes = new List<byte[]>(2)
				{
					Convert.FromBase64String(key),
					Convert.FromBase64String(iv)
				};

				KeysManager.AddSymetricKey(clientIdentity, bytes);
			}
		}

		private static Uri BuildUrl(string endPoint)
		{
			return new Uri(ConfigurationManager.AppSettings["URL"] + endPoint);
		}
	}
}