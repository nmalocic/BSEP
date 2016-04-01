using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using BSEP.Common;
using BSEP.Business;

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

		public static WebRequest PrepareWebRequestWithCertificate(Uri uri, byte[] certificate, string myName)
		{
			WebRequest request = PrepareWebRequest(uri);
			request.Headers.Add(Constants.HeaderKeys.Certificate, Convert.ToBase64String(certificate));
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