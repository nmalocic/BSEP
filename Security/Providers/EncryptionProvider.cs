using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Security
{
	public static class EncryptionProvider
	{
		public static void Encrypt(XmlDocument doc, string elementName, SymmetricAlgorithm key)
		{
			// Check the arguments.  
			if (doc == null)
				throw new ArgumentNullException("Doc");
			if (elementName == null)
				throw new ArgumentNullException("ElementToEncrypt");
			if (key == null)
				throw new ArgumentNullException("Alg");

			////////////////////////////////////////////////
			// Find the specified element in the XmlDocument
			// object and create a new XmlElemnt object.
			////////////////////////////////////////////////
			XmlElement elementToEncrypt = doc.GetElementsByTagName(elementName)[0] as XmlElement;
			// Throw an XmlException if the element was not found.
			if (elementToEncrypt == null)
			{
				throw new XmlException("The specified element was not found");

			}

			//////////////////////////////////////////////////
			// Create a new instance of the EncryptedXml class 
			// and use it to encrypt the XmlElement with the 
			// symmetric key.
			//////////////////////////////////////////////////

			EncryptedXml eXml = new EncryptedXml();

			byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, key, false);
			////////////////////////////////////////////////
			// Construct an EncryptedData object and populate
			// it with the desired encryption information.
			////////////////////////////////////////////////

			EncryptedData edElement = new EncryptedData();
			edElement.Type = EncryptedXml.XmlEncElementUrl;

			// Create an EncryptionMethod element so that the 
			// receiver knows which algorithm to use for decryption.
			// Determine what kind of algorithm is being used and
			// supply the appropriate URL to the EncryptionMethod element.

			string encryptionMethod = null;

			if (key is TripleDES)
			{
				encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
			}
			else if (key is DES)
			{
				encryptionMethod = EncryptedXml.XmlEncDESUrl;
			}
			if (key is Rijndael)
			{
				switch (key.KeySize)
				{
					case 128:
						encryptionMethod = EncryptedXml.XmlEncAES128Url;
						break;
					case 192:
						encryptionMethod = EncryptedXml.XmlEncAES192Url;
						break;
					case 256:
						encryptionMethod = EncryptedXml.XmlEncAES256Url;
						break;
				}
			}
			else
			{
				// Throw an exception if the transform is not in the previous categories
				throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
			}

			edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

			// Add the encrypted element data to the 
			// EncryptedData object.
			edElement.CipherData.CipherValue = encryptedElement;

			////////////////////////////////////////////////////
			// Replace the element from the original XmlDocument
			// object with the EncryptedData element.
			////////////////////////////////////////////////////
			EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
		}

		public static void Decrypt(XmlDocument doc, SymmetricAlgorithm alg)
		{
			// Check the arguments.  
			if (doc == null)
				throw new ArgumentNullException("doc");
			if (alg == null)
				throw new ArgumentNullException("alg");

			// Find the EncryptedData element in the XmlDocument.
			XmlElement encryptedElement = doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;

			// If the EncryptedData element was not found, throw an exception.
			if (encryptedElement == null)
			{
				throw new XmlException("The EncryptedData element was not found.");
			}

			// Create an EncryptedData object and populate it.
			EncryptedData edElement = new EncryptedData();
			edElement.LoadXml(encryptedElement);

			// Create a new EncryptedXml object.
			EncryptedXml exml = new EncryptedXml();

			// Decrypt the element using the symmetric key.
			byte[] rgbOutput = exml.DecryptData(edElement, alg);

			// Replace the encryptedData element with the plaintext XML element.
			exml.ReplaceData(encryptedElement, rgbOutput);
		}
	}
}
