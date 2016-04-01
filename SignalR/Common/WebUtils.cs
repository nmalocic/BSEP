using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using BSEP.Common;

namespace SignalR.Common
{
	public static class WebUtils
	{
		public static string ReadHeaderValue(this HttpRequestMessage source, string headerName)
		{
			return source.Headers.GetValues(headerName).First();
		}
		public static string ReadHeaderValue(this WebResponse source, string headerName)
		{
			return source.Headers[headerName];
		}

		public static WebRequest PrepareRequestWithMessage(Uri uri, string message)
		{
			WebRequest request = PrepareWebRequest(uri);
			request.Headers.Add(Constants.HeaderKeys.Message, message);
			return request;
		}

		public static WebRequest PrepareWebRequestWithCertificate(Uri uri, X509Certificate2 certificate, string myName)
		{
			WebRequest request = PrepareWebRequest(uri);
			request.Headers.Add(Constants.HeaderKeys.Certificate, Convert.ToBase64String(certificate.RawData));
			request.Headers.Add(Constants.HeaderKeys.SenderIdenttiy, Identity.UserName);
			return request;
		}

		public static WebRequest PrepareWebRequest(Uri url)
		{
			var request = WebRequest.Create(url);
			request.ContentLength = 0;
			request.ContentType = "text/xml";
			request.Method = "POST";

			return request;
		}
	}
}