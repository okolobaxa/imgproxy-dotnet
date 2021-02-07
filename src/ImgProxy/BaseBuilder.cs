using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ImgProxy
{
    public class BaseBuilder
    {
        private byte[] _binaryKey;
        private byte[] _binarySalt;
        private string _host;

        protected FormatOption FormatOption;
        protected readonly Dictionary<string, ImgProxyOption> Options = new Dictionary<string, ImgProxyOption>();

        protected void AddOption(ImgProxyOption option)
        {
            switch (option)
            {
                case null:
                    throw new ArgumentNullException(nameof(option));
                case FormatOption formatOption:
                    FormatOption = formatOption;
                    break;
                default:
                    Options[option.GetType().Name] = option;
                    break;
            }
        }
        
        protected void WithEndpoint(string host)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentNullException(nameof(host));
            
            _host = host.TrimEnd('/');
        }

        protected void WithCredentials(string key, string salt)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

            if (key.Length % 2 == 1)
                throw new ArgumentException("Invalid key. The key cannot have an odd number of digits", nameof(key));

            if (salt.Length % 2 == 1)
                throw new ArgumentException("Invalid salt. The salt cannot have an odd number of digits", nameof(salt));

            _binaryKey = HexHelper.HexStringToByteArray(key);
            _binarySalt = HexHelper.HexStringToByteArray(salt);
        }

        private string GetSignature(string path)
        {
            if (_binaryKey == null || _binarySalt == null)
            {
                return "insecure";
            }
            
            var pathBytes = Encoding.UTF8.GetBytes(path);

            var passwordWithSaltByteArray = new byte[pathBytes.Length + _binarySalt.Length];
            Buffer.BlockCopy(_binarySalt, 0, passwordWithSaltByteArray, 0, _binarySalt.Length);
            Buffer.BlockCopy(pathBytes, 0, passwordWithSaltByteArray, _binarySalt.Length, pathBytes.Length);

            using var hmac = new HMACSHA256(_binaryKey);

            var digestBytes = hmac.ComputeHash(passwordWithSaltByteArray);
            
            return HexHelper.ByteArrayToUrlSafeBase64(digestBytes);
        }
        
        protected string BuildWithFormatAndOptions(string url, bool encode, Dictionary<string, ImgProxyOption> dict, FormatOption formatOption)
        {
            var processingOptions = string.Join("/", dict.Values);

            string path;

            if (encode)
            {
                path = formatOption != null
                    ? $"/{processingOptions}/{HexHelper.StringToSafeBase64(url)}.{formatOption.Format}"
                    : $"/{processingOptions}/{HexHelper.StringToSafeBase64(url)}";
            }
            else
            {
                path = formatOption != null
                    ? $"/{processingOptions}/plain/{url}@{formatOption.Format}"
                    : $"/{processingOptions}/plain/{url}";
            }

            var signature = GetSignature(path);

            return $"{_host}/{signature}{path}";
        }
    }
}
