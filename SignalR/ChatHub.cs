using System;
using Microsoft.AspNet.SignalR;
using Security.SecureKeys;
using BSEP.Business;

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
				new RequestHandler()
                    .ExchangeCertificates();
			}
			catch (Exception e)
			{
				
			}
		}

		public void Exchange()
		{
			try
			{
                new RequestHandler()
                    .ExchangeSymetricKey();
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