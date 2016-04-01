using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Security.SecureKeys
{
	public static class KeysManager
	{
		private static readonly IDictionary<string, X509Certificate2> _publicKeys= new Dictionary<string, X509Certificate2>();
		private static readonly IDictionary<string, RijndaelManaged> _simetricKeys = new Dictionary<string, RijndaelManaged>();
		private static X509Certificate2 _key;

		public static void InitializeCertificate(string name)
		{
			try
			{
				_key = GenerateCertificate.CreateSelfSignedCertificate(name);
			}
			catch (Exception ex)
			{
				
			}
		}

		/// <summary>
		/// returns public key to the user who requested it
		/// </summary>
		/// <returns>public key</returns>
		public static X509Certificate2 GetMyCertificate() => _key;

		public static X509Certificate2 GetMyKey() => _key;

		public static RSACryptoServiceProvider GetMyPrivteKey() => (RSACryptoServiceProvider) _key.PrivateKey;

		public static RijndaelManaged GetCurrentSymetricKeyForClient(string client)
		{
			if(!_simetricKeys.ContainsKey(client))
				throw  new ArgumentException("There is no symmetric key for current client");

			return _simetricKeys[client];
		}

		public static X509Certificate2 GetClientKey(string client)
		{
			return _publicKeys[client];
		}

		public static void AddPublicKeyToList(string client, X509Certificate2 key)
		{
			_publicKeys[client] = key;
		}

		public static Tuple<string, string> GenerateSymetricKey(string client)
		{
			if(!_publicKeys.ContainsKey(client))
				throw new InvalidOperationException("Can't encrypt symetric key");

			RSACryptoServiceProvider clientsPublicKey = (RSACryptoServiceProvider)_publicKeys[client].PublicKey.Key;
			RijndaelManaged symmetricKey = new RijndaelManaged();
			_simetricKeys[client] = symmetricKey;

			return new Tuple<string, string>(
				Convert.ToBase64String(clientsPublicKey.Encrypt(symmetricKey.Key, false)),
				Convert.ToBase64String(clientsPublicKey.Encrypt(symmetricKey.IV, false))
				);
		}

		public static void AddSymetricKey(string client, IEnumerable<byte[]> keys)
		{
			RijndaelManaged symetricKey = new RijndaelManaged();

			if(keys.Count()> 2)
				throw new ArgumentException("too much keys");

			var keyToDecrypt = keys.First();
			var IvToDecrypt = keys.Last();

			RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider) _key.PrivateKey;

			var key = privateKey.Decrypt(keyToDecrypt, false);
			var iv = privateKey.Decrypt(IvToDecrypt, false);

			symetricKey.Key = key;
			symetricKey.IV = iv;

			_simetricKeys[client] = symetricKey;
		}
	}
}
