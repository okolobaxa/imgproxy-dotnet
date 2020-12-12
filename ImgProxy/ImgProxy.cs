using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ImgProxy
{
    public static class ImgProxy
    {
        public static ImgProxyBuilder Create => new ImgProxyBuilder();

        public static ImgProxyBuilder Instance { get; set; }
    }

    public class ImgProxyBuilder
    {
        private byte[] _binaryKey;
        private byte[] _binarySalt;

        private string _host;

        private string _pathTemplate;
        private string _resize;
        private int _width;
        private int _height;
        private string _gravity;
        private string _enlarge;

        private string _extension = Extensions.JPG;

        public ImgProxyBuilder WithEndpoint(string host)
        {
            if (!host.EndsWith('/'))
            {
                host += '/';
            }

            _host = host;

            return this;
        }

        public ImgProxyBuilder WithCredentials(string key, string salt)
        {
            if (key.Length % 2 == 1)
                throw new Exception("Invalid key. The key cannot have an odd number of digits");

            if (salt.Length % 2 == 1)
                throw new Exception("Invalid salt. The salt cannot have an odd number of digits");

            _binaryKey = HexadecimalStringToByteArray(key);
            _binarySalt = HexadecimalStringToByteArray(salt);

            return this;
        }

        public ImgProxyBuilder Resize(string resize, int width, int height)
        {
            _width = width;
            _height = height;
            _resize = resize;

            _pathTemplate = RecalculateTemplate();

            return this;
        }

        public ImgProxyBuilder Gravity(string gravity)
        {
            _gravity = gravity;

            _pathTemplate = RecalculateTemplate();

            return this;
        }

        public ImgProxyBuilder Enlarge(bool enlarge)
        {
            _enlarge = enlarge ? "1" : "0";
            _pathTemplate = RecalculateTemplate();

            return this;
        }

        public ImgProxyBuilder Extension(string extension)
        {
            _extension = extension;

            return this;
        }

        private string RecalculateTemplate() => $"/{_resize}/{_width:D}/{_height:D}/{_gravity}/{_enlarge}/";

        public string Build(string url)
        {
            url = StringToSafeBase64String(url);

            var path = $"{_pathTemplate}{url}.{_extension}";
            var pathBytes = Encoding.UTF8.GetBytes(path);

            var passwordWithSaltByteArray = new byte[pathBytes.Length + _binarySalt.Length];
            Buffer.BlockCopy(_binarySalt, 0, passwordWithSaltByteArray, 0, _binarySalt.Length);
            Buffer.BlockCopy(pathBytes, 0, passwordWithSaltByteArray, _binarySalt.Length, pathBytes.Length);

            using var hmac = new HMACSHA256(_binaryKey);

            var digestBytes = hmac.ComputeHash(passwordWithSaltByteArray);
            var urlSafeBase64 = ByteArrayToUrlSafeBase64String(digestBytes);

            return $"{_host}{urlSafeBase64}{path}";
        }

        private static byte[] HexadecimalStringToByteArray(string input)
        {
            var outputLength = input.Length / 2;
            var output = new byte[outputLength];

            using var stringReader = new StringReader(input);

            Span<char> buffer = new char[2];

            for (var i = 0; i < outputLength; i++)
            {
                stringReader.ReadBlock(buffer);

                output[i] = Convert.ToByte(buffer.ToString(), 16);
            }

            return output;
        }

        private static string ByteArrayToUrlSafeBase64String(byte[] byteArray) => Convert.ToBase64String(byteArray)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

        private static string StringToSafeBase64String(string str) =>
            ByteArrayToUrlSafeBase64String(Encoding.ASCII.GetBytes(str));
    }
}
