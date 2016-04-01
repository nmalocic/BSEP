using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
			SignatureProvider.SignXml(xmlDoc, KeysManager.GetMyPrivteKey());

			return Uri.EscapeDataString(xmlDoc.OuterXml);
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
