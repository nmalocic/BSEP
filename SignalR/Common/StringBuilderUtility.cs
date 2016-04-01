using System.Security.Cryptography;
using System.Text;

namespace SignalR.Common
{
	public static class StringBuilderUtility
	{
		public static string GetExchangedPublicKeysHtml(string myKey, string clientsKey)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.Append("<div>");
			strBuilder.Append("My Key: </br>");
			strBuilder.Append(myKey);
			strBuilder.Append("</div>");
			strBuilder.Append("<div>");
			strBuilder.Append("Friends Key: </br>");
			strBuilder.Append(clientsKey);
			strBuilder.Append("</div>");
			return strBuilder.ToString();
		}

		public static string GetKeyUsedToSignInfoHtml(RSACryptoServiceProvider key)
		{
			return GetKeyInfoHtml("Key used to encrypt", key.ToXmlString(false));
		}

		public static string GetKeyUsedToDecryptInfoHtml(RSACryptoServiceProvider key)
		{
			return GetKeyInfoHtml("Key used to decrypt", key.ToXmlString(false));
		}

		private static string GetKeyInfoHtml(string infoMessage, string key)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.Append("<div>");
			strBuilder.Append("<strong>");
			strBuilder.Append(infoMessage);
			strBuilder.Append("</strong>");
			strBuilder.Append("</br>");
			strBuilder.Append(key);
			strBuilder.Append("</div>");
			return strBuilder.ToString();
		}
	}
}