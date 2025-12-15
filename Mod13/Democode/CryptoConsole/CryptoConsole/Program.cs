using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Crypto test");
Console.WriteLine();
var password = "password"; // Demo code only: never hard-code a password!
byte[] salt = Encoding.UTF8.GetBytes("CryptoConsole123");
var filename = "..\\..\\..\\EncryptedData.txt";

using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
{
    Encrypt(stream, password, salt, "Hello World of plaintext!");
}

using (var stream = new FileStream(filename, FileMode.Open))
{
    var decrypted = Decrypt(stream, password, salt);
    Console.WriteLine("Decrypted text:");
    Console.WriteLine(decrypted);
}

var text = @"The current recommendation is to use SHA256, 
SHA384, or SHA512 for all future applications. The MD5, 
RIPEMD160 and SHA1 algorithms are provided for backwards 
compatibility and legacy applications.";
byte[] data = Encoding.UTF8.GetBytes(text);
Console.WriteLine();
Console.WriteLine("SHA512 hash of text:");
var hash = SHA512.HashData(data);
Console.WriteLine(BitConverter.ToString(hash));

static void Encrypt(Stream stream, string password, byte[] salt, string plaintext)
{
    using (var aes = Aes.Create())
    {
        var passwordHash = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA1);
        aes.Key = passwordHash.GetBytes(aes.KeySize / 8);

        // Write the Initialization Vector to the filestream
        stream.Write(aes.IV, 0, aes.IV.Length);

        using (var cryptoStream = new CryptoStream(stream,
                   aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            using (var writer = new StreamWriter(cryptoStream))
            {
                writer.Write(plaintext);
            }
        }
    }
}

static string Decrypt(Stream stream, string password, byte[] salt)
{
    using (var aes = Aes.Create())
    {
        var passwordHash = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA1);
        aes.Key = passwordHash.GetBytes(aes.KeySize / 8);

        // Read the Initialization Vector from the filestream
        byte[] iv = new byte[aes.BlockSize / 8];
        stream.ReadExactly(iv);
        aes.IV = iv;

        using (var cryptoStream = new CryptoStream(stream,
                   aes.CreateDecryptor(), CryptoStreamMode.Read))
        {
            using (var reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd(); ;
            }
        }
    }
}