using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using BSEP.Common;
using Security;
using Security.SecureKeys;

namespace BSEP.Business
{
    public static class MessageGenerator
    {
	    public static string GenerateXmlMesasgeForClient(string sender, string message, string client)
	    {
			// Create a new XML document.
			var xmlDoc = LoadXml(sender, message);

			//encrypt message 
			EncryptionProvider.Encrypt(xmlDoc, 
										Constants.ElementNames.Message, 
										KeysManager.GetCurrentSymetricKeyForClient(client));

			//Sign message
			SignatureProvider.SignXml(xmlDoc, KeysManager.GetMyPrivateKey());

			return Uri.EscapeDataString(xmlDoc.OuterXml);
		}

        public static void ReadXmlMessage(string encodedMessage, out string message, out string sender)
        {
            message = string.Empty;
            sender = string.Empty;
            var xml = Uri.UnescapeDataString(encodedMessage);
            var clientCertificate = KeysManager.GetClientKey(Identity.ChatClientName);
            var keyToDecrypt = (RSACryptoServiceProvider)clientCertificate.PublicKey.Key;

            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            if (!SignatureProvider.VerifyXml(document, keyToDecrypt))
                throw new Exception("");
                //return Request.CreateResponse(HttpStatusCode.ExpectationFailed);

            EncryptionProvider.Decrypt(document, KeysManager.GetCurrentSymetricKeyForClient(Identity.ChatClientName));

            message = document.GetElementsByTagName(Constants.ElementNames.Message)[0].InnerText.Trim();
            sender = document.GetElementsByTagName(Constants.ElementNames.Sender)[0].InnerText.Trim();
        }

		private static XmlDocument LoadXml(string name, string message)
		{
			var xmlDoc = new XmlDocument { PreserveWhitespace = false };
			xmlDoc.LoadXml(LoadTextAndSetValues(name, message));
			return xmlDoc;
		}

		private static string LoadTextAndSetValues(string name, string message)
		{
			string xmlText =
				string.Format(
					File
						.ReadAllText(@"E:\Work\Vega\Projects\SignalR\SignalR\XmlSample\Message.xml"),
					message,
					name);
			return xmlText;
		}
	}
}
