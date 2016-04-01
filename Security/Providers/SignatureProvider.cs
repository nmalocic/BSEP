using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Security.SecureKeys
{
	public static class SignatureProvider
	{
		// Verify the signature of an XML file against an asymmetric 
		// algorithm and return the result.
		public static bool VerifyXml(XmlDocument Doc, RSA Key)
		{
			// Check arguments.
			if (Doc == null)
				throw new ArgumentException("Doc");
			if (Key == null)
				throw new ArgumentException("Key");

			// Create a new SignedXml object and pass it
			// the XML document class.
			SignedXml signedXml = new SignedXml(Doc);

			// Find the "Signature" node and create a new
			// XmlNodeList object.
			XmlNodeList nodeList = Doc.GetElementsByTagName("Signature");

			// Throw an exception if no signature was found.
			if (nodeList.Count <= 0)
			{
				throw new CryptographicException("Verification failed: No Signature was found in the document.");
			}

			// This example only supports one signature for
			// the entire XML document.  Throw an exception 
			// if more than one signature was found.
			if (nodeList.Count >= 2)
			{
				throw new CryptographicException("Verification failed: More that one signature was found for the document.");
			}
			
			// Load the first <signature> node.  
			signedXml.LoadXml((XmlElement)nodeList[0]);

			// Check the signature and return the result.
			var isValid = signedXml.CheckSignature(Key);

			return isValid;
		}

		// Sign an XML file. 
		// This document cannot be verified unless the verifying 
		// code has the key with which it was signed.
		public static void SignXml(XmlDocument xmlDoc, RSA Key)
		{
			// Check arguments.
			if (xmlDoc == null)
				throw new ArgumentException("xmlDoc");
			if (Key == null)
				throw new ArgumentException("Key");


			// Create a SignedXml object.
			SignedXml signedXml = new SignedXml(xmlDoc);

			// Add the key to the SignedXml document.
			signedXml.SigningKey = Key;

			// Create a reference to be signed.
			Reference reference = new Reference();
			reference.Uri = "";

			// Add an enveloped transformation to the reference.
			XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(env);

			// Add the reference to the SignedXml object.
			signedXml.AddReference(reference);

			// Compute the signature.
			signedXml.ComputeSignature();

			// Get the XML representation of the signature and save
			// it to an XmlElement object.
			XmlElement xmlDigitalSignature = signedXml.GetXml();

			// Append the element to the XML document.
			xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
		}
	}
}
