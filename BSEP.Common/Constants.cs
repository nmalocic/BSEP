using System.Security.Cryptography.X509Certificates;

namespace BSEP.Common
{
	public static class Constants
	{
		public static class HeaderKeys
		{
			public const string Certificate = "Cert";
			public const string Message = "Message";
			public const string SymmetricKey = "SymmetricKey";
			public const string SymmetricIv = "SymmetricIv";
			public const string SenderIdenttiy = "UserName";
		}

		public static class ElementNames
		{
			public const string Message = "message";
			public const string Sender = "sender";
			public const string Signature = "Signature";
		}

		public static class Endpoints
		{
			public const string Chat = "Chat";
			public const string ExchangeCertificate = "PublicKey";
			public const string SymmetricKey = "SymmetricKey";
		}
	}
}
