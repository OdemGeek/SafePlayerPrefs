using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace TruuCod.SafePrefsSystem
{
    public static class SPrefs
    {
        #region Secret Keys
        private static string secretKey = "someKey";
        private static byte[] key = new byte[8] { 81, 245, 87, 33, 14, 48, 105, 184 };
        private static byte[] iv = new byte[8] { 7, 82, 212, 199, 39, 2, 4, 22 };
        #endregion

        #region Tools
        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(Md5(key));
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(Md5(key));
        }
        #endregion

        #region Int
        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetString(Md5(key), Encrypt(value.ToString()));
        }

        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (!HasKey(key))
                return defaultValue;
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(Md5(key)));
                int i = int.Parse(s);
                return i;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region Float
        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetString(Md5(key), Encrypt(value.ToString()));
        }

        public static float GetFloat(string key)
        {
            return GetFloat(key, 0);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            if (!HasKey(key))
                return defaultValue;
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(Md5(key)));
                float f = float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                return f;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region String
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(Md5(key), Encrypt(value));
        }

        public static string GetString(string key)
        {
            return GetString(key, "");
        }

        public static string GetString(string key, string defaultValue)
        {
            if (!HasKey(key))
                return defaultValue;
            try
            {
                string s = Decrypt(PlayerPrefs.GetString(Md5(key)));
                return s;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region Encryption
        private static string Encrypt(string s)
        {
            byte[] inputbuffer = Encoding.Unicode.GetBytes(s);
            byte[] outputBuffer = DES.Create().CreateEncryptor(key, iv).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return System.Convert.ToBase64String(outputBuffer);
        }

        private static string Decrypt(string s)
        {
            byte[] inputbuffer = System.Convert.FromBase64String(s);
            byte[] outputBuffer = DES.Create().CreateDecryptor(key, iv).TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        private static string Md5(string s)
        {
            // Can also add a SystemInfo.deviceUniqueIdentifier to prevent access to saves on other devices
            byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(s + secretKey));
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString.PadLeft(32, '0');
        }
        #endregion
    }
}
