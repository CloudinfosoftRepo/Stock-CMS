using log4net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

public class Crypter
{

    private const string AesIV = "XjryPdxoFoAqJVw0xbb+vA==";
    private const string AesKey = "yGk9G4eIJ66yPRTcZLRmsNhn4nudaVtmdG1ZB8LTqNc=";


    public static string Encrypt(string plainText)
    {
        // Check arguments. 
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");


        ////byte[] Key = Encoding.UTF8.GetBytes(AesKey);
        ////byte[] IV = Encoding.UTF8.GetBytes(AesIV);

        byte[] Key = { 36, 83, 76, 84, 65, 75, 69, 48, 73, 48, 69, 89, 48, 84, 79, 67 };
        byte[] IV = { 37, 83, 67, 75, 83, 65, 75, 59, 48, 48, 73, 87, 48, 48, 84, 79 };

        byte[] encrypted;
        // Create an AesManaged object 
        // with the specified key and IV. 
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption. 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }


        // Return the encrypted bytes from the memory stream. 
        return Convert.ToBase64String(encrypted);

    }


    public static string Decrypt(string plainText)
    {

        byte[] pText = System.Convert.FromBase64String(plainText);
        // Check arguments. 
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");

        ////byte[] Key = Encoding.UTF8.GetBytes(AesKey);
        ////byte[] IV = Encoding.UTF8.GetBytes(AesIV);

        byte[] Key = { 36, 83, 76, 84, 65, 75, 69, 48, 73, 48, 69, 89, 48, 84, 79, 67 };
        byte[] IV = { 37, 83, 67, 75, 83, 65, 75, 59, 48, 48, 73, 87, 48, 48, 84, 79 };

        string decrypted;
        // Create an AesManaged object 
        // with the specified key and IV. 
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


            // Create the streams used for encryption. 
            using (MemoryStream msDecrypt = new MemoryStream(pText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream 
                        // and place them in a string.
                        decrypted = srDecrypt.ReadToEnd();
                    }

                }
            }

        }

        // Return the encrypted bytes from the memory stream. 
        return decrypted;

    }

    public static string GenerateHMAC(string ChecksumData, string AppID, string PRIVATE_KEY)
    {

        string CDATA = string.Empty;

        // Verify HMAC-SHA256 Checksum

        if (PRIVATE_KEY != string.Empty)
        {

            byte[] key = Encoding.UTF8.GetBytes(PRIVATE_KEY);
            byte[] value = Encoding.UTF8.GetBytes(ChecksumData);
            using (var hmac = new System.Security.Cryptography.HMACSHA256(key))
            {
                CDATA = Convert.ToBase64String(hmac.ComputeHash(value));
            }
        }
        return CDATA;
    }
}
public static class RSACrypter
{
    private static readonly string PrivateKey = "<RSAKeyValue><Modulus>0BVz5vqaHs5wfYndyjFGffBNQ/8bMPAVZoF0IiYzKFyj9CWe6CupaMzp3ixaUaz6FqKhEfCpM/6u0AU2fVg7W0yRxcHizHDxNKc/SjbuRFeVde3qQA3JTBnO5gZ6yze+6QAS4KOu1bylEb+QDmriOHwbYLvFKjNBoeSuWnWEN7BBEFQnp092N1VJ2CI1GP7VWpV/xtr9Z17/vrz4cfhExMCIeZxFV51S7BzR6+B8/PdFcrmTTNeFn8lAh1rvU0q1O4U4YaDYR/PSG1kQXPin627X7wSryc/ZL6wZyztmDTfekwu2B4zHLnfcQMqpi4u2hC36RDj/JVhFGyqUdJS+KQ==</Modulus><Exponent>AQAB</Exponent><P>42OQf240ACDAw7/zpdlocyDB6KcbnzqQ+48TVNLxRf4w2ywnEmZFvXDehIK+15MuHzcPHSWZvclY3VVH0PXvs5oS4U7a5/FPMEXIpd0lFysXSxV+EvcFzcbUXVF9nfPzVADy7Z41i0cCJtmEq0kQFDhrSK9gSf81rBQdULOfl9U=</P><Q>6kQMzohew7hcENu7fZSpXf1PJC4qVma4+skJaKnREVHp1eotIJZiq10CZbCNjXOhbq93yZbB/WEemGIHaNp/SBHhm1C/WH1ReUA1ePH7D2GtDPbi8n9KQyJWmXrvBFbNJxMKEt7my5OZXnMiJmiEtJP2QpdBTsH6t9k9SXUeKwU=</Q><DP>3GauMYX86z7T+Si+HjwrrKPMsBgBgwX+b4aPw0EvUZt/eYFDYec1o9iaJjCgkE19gA4bHTJL4ZQfCXHWyWkIbLpjoHWRYw4Xpn+Na499mwZObp2ndPRwcLNPNiB9tLM3YG4HCWC3H4e1QzjFCk0oQ6EoA+zRVQgE2wUoIQfqIf0=</DP><DQ>VrYtj10Gxqc83LiYo5kdP779RkUYQrbvDVTQeL8ytZh+V0HAxmkjyI46zu0U2CDOxiuIyUNWNrV2H61ZVMRbjI+h1F2FFfn86EaOBGB9L2pWJZBU3LGlCUhyiYjpY8/FTAtOFUIQZ5YQlNdE8raJOFX0Nx3JfJ9scBSnDMOfEQk=</DQ><InverseQ>wkxVaHkmYDd2VeArqumVmkgqFfKTit6QdQwdwxNUiWDQlA/lbHo7aaSoZowwSogfVFO0TDxIb2IS2k3AFBeVwb4GiKBraKqMLjs/bSxzIvTBOaYWiJVthtD989LR1DBgYW0niN6vvoraMYoK9Y8MaHBhtitzosf0hdk3O8ITgR4=</InverseQ><D>LPex3iTieQ1WWH7aeAc5i15KqErhQqxjh40qvnWAYTTR9qFJq7VBzUTZNnrhwtxNiGY5RjH6suKTPx+dRK20WBjeOL0baOBwc9c/0Wc5CvpdcYiKcrC4A3LhWCrmzvNbVal/Kt4cmpKDnqgF9uOowxmo5fj2FRZSFO0hM6nJOqo/xQ2c0jmZF0z+ouNgGk7Rt01NNMFm1mr15uByFUtD500ulIU++DqMvJ9oBma0ney72PebfLbPzBYyrM6W+XJH8ctOlBHOL3gt4UbvxB5ZaM6SR/VjLJDFBF2EZUnkGqqnzQ2t9D6zu5+2h9iWHUOkYC+0/acCCNtoaXjUR57sHQ==</D></RSAKeyValue>";

    private static readonly string PublicKey = "<RSAKeyValue><Modulus>0BVz5vqaHs5wfYndyjFGffBNQ/8bMPAVZoF0IiYzKFyj9CWe6CupaMzp3ixaUaz6FqKhEfCpM/6u0AU2fVg7W0yRxcHizHDxNKc/SjbuRFeVde3qQA3JTBnO5gZ6yze+6QAS4KOu1bylEb+QDmriOHwbYLvFKjNBoeSuWnWEN7BBEFQnp092N1VJ2CI1GP7VWpV/xtr9Z17/vrz4cfhExMCIeZxFV51S7BzR6+B8/PdFcrmTTNeFn8lAh1rvU0q1O4U4YaDYR/PSG1kQXPin627X7wSryc/ZL6wZyztmDTfekwu2B4zHLnfcQMqpi4u2hC36RDj/JVhFGyqUdJS+KQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";


    private static readonly string PublicKeyPME = "-----BEGIN PUBLIC KEY-----\r\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0BVz5vqaHs5wfYndyjFG\r\nffBNQ/8bMPAVZoF0IiYzKFyj9CWe6CupaMzp3ixaUaz6FqKhEfCpM/6u0AU2fVg7\r\nW0yRxcHizHDxNKc/SjbuRFeVde3qQA3JTBnO5gZ6yze+6QAS4KOu1bylEb+QDmri\r\nOHwbYLvFKjNBoeSuWnWEN7BBEFQnp092N1VJ2CI1GP7VWpV/xtr9Z17/vrz4cfhE\r\nxMCIeZxFV51S7BzR6+B8/PdFcrmTTNeFn8lAh1rvU0q1O4U4YaDYR/PSG1kQXPin\r\n627X7wSryc/ZL6wZyztmDTfekwu2B4zHLnfcQMqpi4u2hC36RDj/JVhFGyqUdJS+\r\nKQIDAQAB\r\n-----END PUBLIC KEY-----\r\n";




    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly string EncodeKey = "VmYq3t6w9z$C&F)J";

    public static string GetPublicKey()
    {
        return PublicKeyPME;
    }

    public static string GetDecryptedText(string password)
    {
        try
        {
            // Get encrypted password.
            var base64 = password.Trim();

            var bytes = System.Convert.FromBase64String(base64);
            // Get new RSA provider.
            var rsa = GetNewRsaProvider();
            // Import Private RSA key.                
            rsa.FromXmlString(PrivateKey);
            // Decrypt password.
            var data = rsa.Decrypt(bytes, true);
            // Convert bytes to UTF8
            var pass = System.Text.Encoding.UTF8.GetString(data);
            // Show password to user (just for debug purposes).

            return pass;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    private static System.Security.Cryptography.RSACryptoServiceProvider GetNewRsaProvider(int dwKeySize = 1024)
    {
        // Tell IIS to use Machine Key store or creation of RSA service provider will fail.
        var cspParams = new CspParameters();
        cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
        // Create a new instance of RSACryptoServiceProvider.
        return new System.Security.Cryptography.RSACryptoServiceProvider(dwKeySize, cspParams);
    }

    public static string GetEncryptedText(string plainText)
    {
        try
        {

            var data = System.Text.Encoding.UTF8.GetBytes(plainText);
            // Get public key.
            var xmlParams = PublicKey;
            // Create a new instance of RSACryptoServiceProvider.
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            // Import parameters from xml.
            rsa.FromXmlString(xmlParams);
            // Encrypt data (use OAEP padding).
            var encryptedBytes = rsa.Encrypt(data, true);
            // Convert encrypted data to Base64.
            var encryptedString = System.Convert.ToBase64String(encryptedBytes);
            // Replace plain password with encrypted data.

            return encryptedString;

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    public static string DecodeText(string EncodedText)
    {
        try
        {
            byte[] data = Convert.FromBase64String(EncodedText);
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            throw ex;
        }

        return string.Empty;

    }

    public static string EncodeCompanionText(string plaintext)
    {
        try
        {
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
            //var encodetext = System.Convert.ToBase64String(plainTextBytes);
            //return encodetext;

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(EncodeKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
            tripleDES.Clear();
            var encodetext = System.Convert.ToBase64String(plainTextBytes);
            // return encodetext;
            return Convert.ToBase64String(resultArray, 0, resultArray.Length); ;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            throw ex;
        }
        return string.Empty;
    }

    public static string DecodeCompanionText(string EncodedText)
    {
        try
        {
            byte[] data = Convert.FromBase64String(EncodedText);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(EncodeKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            throw ex;
        }

        return string.Empty;

    }

}

public static class RSACrypterNew
{
    private static readonly string PrivateKey = "<RSAKeyValue><Modulus>hJTtBrnKDElJG6QOFRK0foScGa/AmuRjSTffU2m/WPXrJmVlcY9DWUYZukz02jm2fkoxUJ6UyAqrDox91ntvHSv7D29sh5WjsmD0q6ttDPBSn7QH31OVslNeygNmYkNCx4ffJCOb/NxCLEhd3bY4QDfNylQayOTQJPQamZF5O7k=</Modulus><Exponent>AQAB</Exponent><P>2fptgywJtMMpy3LhWbDbqGavQtvlejf5fVfmzWtLz7CtLvUM7L1GxuTq1ul3jfOpOuQ1mIBk5TRfZU1aIj+2yQ==</P><Q>m7U2zGlflFgIAUFToKYRdJGDPXAGXXm2zBikdblS4XTrP9o/rrdjk/sKcfibGOwmZzRl/bI/zAKy75wFsTqlcQ==</Q><DP>fAIAc3OGQhQVnuzIT2JSVMe2RW8cah+WDD0JzSuRgaKdo+09YSF9NoHdKE7B+Tv5tstFwnCo9cyJyPPspzdgIQ==</DP><DQ>QGBz/5+xfMvIw3wW71y56tAeys/+Ubd49HlbxbCRq9WDTisWGU6JRS2N7AE1XuCEIDdx06pW5ipXgOmpENWGgQ==</DQ><InverseQ>ZXAPAPYV/5X6m/D73n6iZU5D2gEWJOc6yd9kWPlQP7O9NRLZLlg+wzql0ZM+XHNRtCHXNcpeh+zX6Y+F2h1iYw==</InverseQ><D>C0+FNRiugwPRh9kkFmolGdIIWyyiOtSXH8zzcXNJDrbUG890qw50yW3wjfM4xFE+H2IG+QC+Yp5+X1xe8+hN30hbAMWTnZMb5hQ/t6B96bMnOL+8HhaIqh/cwl+2fI+/i6AQuOt1p1VAYr7C+dzSR7CN53HuFwCL44nPMkTiNIE=</D></RSAKeyValue>";
    private static readonly string PublicKey = "<RSAKeyValue><Modulus>hJTtBrnKDElJG6QOFRK0foScGa/AmuRjSTffU2m/WPXrJmVlcY9DWUYZukz02jm2fkoxUJ6UyAqrDox91ntvHSv7D29sh5WjsmD0q6ttDPBSn7QH31OVslNeygNmYkNCx4ffJCOb/NxCLEhd3bY4QDfNylQayOTQJPQamZF5O7k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static string GetPublicKey()
    {
        return PublicKey;
    }

    public static string GetDecryptedText(string password)
    {
        try
        {
            // Get encrypted password.
            var base64 = password.Trim();

            var bytes = System.Convert.FromBase64String(base64);
            // Get new RSA provider.
            var rsa = GetNewRsaProvider();
            // Import Private RSA key.                
            rsa.FromXmlString(PrivateKey);
            // Decrypt password.
            var data = rsa.Decrypt(bytes, true);
            // Convert bytes to UTF8
            var pass = System.Text.Encoding.UTF8.GetString(data);
            // Show password to user (just for debug purposes).

            return pass;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    private static System.Security.Cryptography.RSACryptoServiceProvider GetNewRsaProvider(int dwKeySize = 1024)
    {
        // Tell IIS to use Machine Key store or creation of RSA service provider will fail.
        var cspParams = new CspParameters();
        cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
        // Create a new instance of RSACryptoServiceProvider.
        return new System.Security.Cryptography.RSACryptoServiceProvider(dwKeySize, cspParams);
    }

    public static string GetEncryptedText(string plainText)
    {
        try
        {

            var data = System.Text.Encoding.UTF8.GetBytes(plainText);
            // Get public key.
            var xmlParams = PublicKey;
            // Create a new instance of RSACryptoServiceProvider.
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            // Import parameters from xml.
            rsa.FromXmlString(xmlParams);
            // Encrypt data (use OAEP padding).
            var encryptedBytes = rsa.Encrypt(data, true);
            // Convert encrypted data to Base64.
            var encryptedString = System.Convert.ToBase64String(encryptedBytes);
            // Replace plain password with encrypted data.

            return encryptedString;

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }
}

public static class SSORSACrypter
{
    private static readonly string PrivateKey = "<RSAKeyValue><Modulus>qSF4CNExX3NFoJk0LTQ7HRLlBj8BwYSLqImfzKRNzs20YjnWxO5c8D0yQ/UPQEggEDnbG8hv7fuTfOYjupw9YoEdA5PRJDvjIvUbP5qR3rcjkI1xlGb4jY6I8VYr0hrxRJaGmClBFLRe2+s8uLPYsaZBAjoFuJG9No0D4Q/dQCU=</Modulus><Exponent>AQAB</Exponent><P>1rlSGMXfMWxdxt2Dw8XHSkf4dtAVAcYi5c1QdmXpbCilko8I26Zt3xTnj8uwGPPBzqFU2NvZmUyAafbOus2oGQ==</P><Q>yaR70YNoc7qLNs4PzESkmVVothma/73tEBD+Zsol/gGkLz7r/mwnMcIuUFkqokWW07ftxROMiVZpXc0lDgXJ7Q==</Q><DP>D0E7Q+dK5Af+4ZH7On1imzRhwo8l7upUsF7VyJOU8coxeSgQSQqZ469ohB7EdAFdU4DCtmVoICv/M4gFR+A0gQ==</DP><DQ>QE7B4la7aDU1Etxy811s9S3lADZ4VVmYEkipKdUMes7BMwQpNXH+qgLYzG0ziJ8S9CCzpueOKXqyjeTQDYHRHQ==</DQ><InverseQ>XM5MjEfmzXx2BlTvTIXhKFJgctE+iCntpWjsc7NbR6rz3bHujXunbwTHIF7e98OySi25nPYkIhrMzNBvBhYbqg==</InverseQ><D>ExGX8/6J/4YCmqnLTTFvXMbNlZey/fuQt27U2oDH2QIuTUjSIx/w0JFFwo21q3y2SL4NKyO9IurTZSiZbKfC79JHHKjBjahdDngMWixRuyuIHH1YtPKPYL2+t1TGpdvCg3NKYfdCedeZLkX/6eHnLWUYUu0pQyANqgwvOfRhcaE=</D></RSAKeyValue>";
    private static readonly string PublicKey = "<RSAKeyValue><Modulus>qSF4CNExX3NFoJk0LTQ7HRLlBj8BwYSLqImfzKRNzs20YjnWxO5c8D0yQ/UPQEggEDnbG8hv7fuTfOYjupw9YoEdA5PRJDvjIvUbP5qR3rcjkI1xlGb4jY6I8VYr0hrxRJaGmClBFLRe2+s8uLPYsaZBAjoFuJG9No0D4Q/dQCU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static string GetPublicKey()
    {
        return PublicKey;
    }

    public static string GetDecryptedText(string password)
    {
        try
        {
            // Get encrypted password.
            var base64 = password.Trim();

            var bytes = System.Convert.FromBase64String(base64);
            // Get new RSA provider.
            var rsa = GetNewRsaProvider();
            // Import Private RSA key.                
            rsa.FromXmlString(PrivateKey);
            // Decrypt password.
            var data = rsa.Decrypt(bytes, true);
            // Convert bytes to UTF8
            var pass = System.Text.Encoding.UTF8.GetString(data);
            // Show password to user (just for debug purposes).

            return pass;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    private static System.Security.Cryptography.RSACryptoServiceProvider GetNewRsaProvider(int dwKeySize = 1024)
    {
        // Tell IIS to use Machine Key store or creation of RSA service provider will fail.
        var cspParams = new CspParameters();
        cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
        // Create a new instance of RSACryptoServiceProvider.
        return new System.Security.Cryptography.RSACryptoServiceProvider(dwKeySize, cspParams);
    }

    public static string GetEncryptedText(string plainText)
    {
        try
        {

            var data = System.Text.Encoding.UTF8.GetBytes(plainText);
            // Get public key.
            var xmlParams = PublicKey;
            // Create a new instance of RSACryptoServiceProvider.
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            // Import parameters from xml.
            rsa.FromXmlString(xmlParams);
            // Encrypt data (use OAEP padding).
            var encryptedBytes = rsa.Encrypt(data, true);
            // Convert encrypted data to Base64.
            var encryptedString = System.Convert.ToBase64String(encryptedBytes);
            // Replace plain password with encrypted data.

            return encryptedString;

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }
}

public static class AESKeyGenerator
{
    public static string GenerateAESIV()
    {
        using (var aes = Aes.Create())
        {
            aes.GenerateIV();
            return Convert.ToBase64String(aes.IV);
        }
    }

    public static string GenerateAESKey()
    {
        using (var aes = Aes.Create())
        {
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }
}

public static class RSAKeyGenerator
{
    public static string GenerateRSAKeys()
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            return rsa.ToXmlString(true); // Include private parameters
        }
    }
}
