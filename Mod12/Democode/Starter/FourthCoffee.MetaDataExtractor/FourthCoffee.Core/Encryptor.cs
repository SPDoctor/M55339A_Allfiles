using FourthCoffee.Core.CustomAttributes;
using System.Security.Cryptography;
using System.Text;
namespace FourthCoffee.Core
{
    [DeveloperInfo("davidh@fourthcoffee.com", 5)]
    public class Encryptor : IDisposable
    {
        private byte[] _salt;
        private Aes _algorithm;
        [DeveloperInfo("hollyh@fourthcoffee.com", 5)]
        public Encryptor(string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                throw new NullReferenceException();
            }
            this._salt = Encoding.Unicode.GetBytes(salt);
            this._algorithm = Aes.Create();
        }
        [DeveloperInfo("danp@fourthcoffee.com", 2)]
        public byte[] Encrypt(byte[] bytesToEncypt, string password)
        {
            Rfc2898DeriveBytes passwordHash = this.GeneratePasswordHash(password);
            byte[] rgbKey = this.GenerateKey(passwordHash);
            byte[] rgbIV = this.GenerateIV(passwordHash);
            ICryptoTransform transformer = this._algorithm.CreateEncryptor(rgbKey, rgbIV);
            return this.TransformBytes(transformer, bytesToEncypt);
        }
        [DeveloperInfo("danp@fourthcoffee.com", 3)]
        public byte[] Decrypt(byte[] bytesToDecypt, string password)
        {
            Rfc2898DeriveBytes passwordHash = this.GeneratePasswordHash(password);
            byte[] rgbKey = this.GenerateKey(passwordHash);
            byte[] rgbIV = this.GenerateIV(passwordHash);
            ICryptoTransform transformer = this._algorithm.CreateDecryptor(rgbKey, rgbIV);
            return this.TransformBytes(transformer, bytesToDecypt);
        }
        private Rfc2898DeriveBytes GeneratePasswordHash(string password)
        {
            return new Rfc2898DeriveBytes(password, this._salt, 1000, HashAlgorithmName.SHA1);
        }
        private byte[] GenerateKey(Rfc2898DeriveBytes passwordHash)
        {
            return passwordHash.GetBytes(this._algorithm.KeySize / 8);
        }
        private byte[] GenerateIV(Rfc2898DeriveBytes passwordHash)
        {
            return passwordHash.GetBytes(this._algorithm.BlockSize / 8);
        }
        private byte[] TransformBytes(ICryptoTransform transformer, byte[] bytesToTransform)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transformer, CryptoStreamMode.Write);
            cryptoStream.Write(bytesToTransform, 0, bytesToTransform.Length);
            cryptoStream.FlushFinalBlock();
            byte[] result = memoryStream.ToArray();
            cryptoStream.Close();
            memoryStream.Close();
            return result;
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this._algorithm != null)
                {
                    this._algorithm.Dispose();
                }
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
