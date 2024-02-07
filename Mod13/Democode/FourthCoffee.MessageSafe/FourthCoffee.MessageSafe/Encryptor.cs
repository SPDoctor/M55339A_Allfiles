using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FourthCoffee.MessageSafe
{
    /// <summary>
    /// Represents the <see cref="FourthCoffee.ExceptionLogger.MessageSafe" /> class in the object model.
    /// </summary>
    class Encryptor : IDisposable
    {
        byte[] _salt;
        Aes _algorithm;

        /// <summary>
        /// Create an instance of the Encryptor class.
        /// </summary>
        /// <param name="salt">The salt to use when encrypting and decrypting messages.</param>
        public Encryptor(string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new NullReferenceException();

            _salt = Encoding.Unicode.GetBytes(salt);

            // TODO: 01: Instantiate the _algorithm object.
            _algorithm = Aes.Create();
        }

        /// <summary>
        /// Encrypts a series of bytes.
        /// </summary>
        /// <param name="bytesToEncypt">The bytes to encrypt.</param>
        /// <param name="password">The password used to encrypt the bytes.</param>
        public byte[] Encrypt(byte[] bytesToEncypt, string password)
        {
            var passwordHash = GeneratePasswordHash(password);

            var key = GenerateKey(passwordHash);

            var iv = GenerateIV(passwordHash);

            // TODO: 11: Use the _algorithm object to create a ICryptoTransform encryptor object.
            var transformer = _algorithm.CreateEncryptor(key, iv);

            // TODO: 12: Invoke the TransformBytes method and return the encrypted bytes.
            return TransformBytes(transformer, bytesToEncypt);

        }

        /// <summary>
        /// Decrypts a series of bytes.
        /// </summary>
        /// <param name="bytesToEncypt">The bytes to decrypt.</param>
        /// <param name="password">The password used to decrypt the bytes.</param>
        public byte[] Decrypt(byte[] bytesToDecypt, string password)
        {
            var passwordHash = GeneratePasswordHash(password);

            var key = GenerateKey(passwordHash);

            var iv = GenerateIV(passwordHash);

            // TODO: 13: Use the _algorithm object to create a ICryptoTransform decryptor object.
            var transformer = _algorithm.CreateDecryptor(key, iv);

            // TODO: 14: Invoke the TransformBytes method and return the decrypted bytes.
            return TransformBytes(transformer, bytesToDecypt);
        }

        private Rfc2898DeriveBytes GeneratePasswordHash(string password)
        {
            // TODO: 03: Derive a Rfc2898DeriveBytes object from the password and salt.
            return new Rfc2898DeriveBytes(password, _salt);
        }

        private byte[] GenerateKey(Rfc2898DeriveBytes passwordHash)
        {
            // TODO: 04: Generate the secret key by using the Rfc2898DeriveBytes object.
            return passwordHash.GetBytes(_algorithm.KeySize / 8);
        }

        private byte[] GenerateIV(Rfc2898DeriveBytes passwordHash)
        {
            // TODO: 05: Generate the IV by using the Rfc2898DeriveBytes object.
            return passwordHash.GetBytes(_algorithm.BlockSize / 8);
        }

        private byte[] TransformBytes(ICryptoTransform transformer, byte[] bytesToTransform)
        {
            // TODO: 06: Create a new MemoryStream object.
            var bufferStream = new MemoryStream();

            // TODO: 07: Create a new CryptoStream object.
            var cryptoStream = new CryptoStream(
                bufferStream,
                transformer,
                CryptoStreamMode.Write);

            // TODO: 08: Write the bytes to the CryptoStream object.
            cryptoStream.Write(bytesToTransform, 0, bytesToTransform.Length);
            cryptoStream.FlushFinalBlock();

            // TODO: 09: Read the transformed bytes from the MemoryStream object.
            var transformedBytes = bufferStream.ToArray();

            // TODO: 10: Close the CryptoStream and MemoryStream objects.
            cryptoStream.Close();
            bufferStream.Close();

            return transformedBytes;
        }

        #region IDisposable Members

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // TODO: 02: Dispose of the _algorithm object.
                if (_algorithm != null)
                {
                    _algorithm.Dispose();
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
