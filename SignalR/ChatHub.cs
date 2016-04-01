using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.AspNet.SignalR;
using Security;
using Security.SecureKeys;
using SignalR.Common;
using BSEP.Common;

namespace SignalR
{
	public class ChatHub : Hub
	{
		public void Send(string name, string message)
		{
			try
			{
				new RequestHandler()
					.ProcessSendMessageRequest(name, message);
			}
			catch (Exception ex)
			{
				
			}
			// Call the broadcastMessage method to update clients.
			Clients.All.broadcastMessageValue(name, message);
		}
		
		public void Take()
		{
			try
			{
				new RequestHandler().ExchangeCertificates();
			}
			catch (Exception e)
			{
				
			}
		}

		public void Exchange()
		{
			try
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
			catch (Exception ex)
			{
				
			}
		}

		public void SetupName(string userName)
		{
			Identity.UserName = userName;
			KeysManager.InitializeCertificate(userName);
		}




	}
}