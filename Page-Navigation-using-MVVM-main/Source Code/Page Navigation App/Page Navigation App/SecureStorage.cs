using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public class SecureStorage
{
    private const string EncryptionKey = "YourEncryptionKey"; // Replace with a strong, unique key

    public static void StoreCredentials(string username, string password)
    {
        try
        {
            // Encrypt the username and password before storing
            string encryptedUsername = EncryptString(username);
            string encryptedPassword = EncryptString(password);

            // Store the encrypted credentials in a file
            string filePath = "path_to_your_credentials_file.txt";
            File.WriteAllText(filePath, $"{encryptedUsername}\n{encryptedPassword}");

            // For demonstration purposes, show a message box
         
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error storing credentials: {ex.Message}");
            // Handle the exception (log, notify, etc.)
        }
    }




    private static string EncryptString(string input)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GenerateValidKey(EncryptionKey);

            aesAlg.IV = new byte[16]; // Initialization Vector, should be unique for each encrypted string

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                    {
                        swEncrypt.Write(input);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    private static byte[] GenerateValidKey(string key)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }

    public static bool TryGetStoredCredentials(out string username, out string password)
    {
        try
        {
            // Read the stored credentials from the file
            string filePath = "path_to_your_credentials_file.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length == 2)
                {
                    username = DecryptString(lines[0]);
                    password = DecryptString(lines[1]);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error retrieving stored credentials: {ex.Message}");
            // Handle the exception (log, notify, etc.)
        }

        // If anything goes wrong or no credentials are found, return false
        username = null;
        password = null;
        return false;
    }


    private static string DecryptString(string input)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GenerateValidKey(EncryptionKey);
            aesAlg.IV = new byte[16];

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }




}