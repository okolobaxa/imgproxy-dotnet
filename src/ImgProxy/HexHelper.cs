using System;
using System.IO;
using System.Text;

namespace ImgProxy
{
    internal static class HexHelper
    {
        public static byte[] HexStringToByteArray(string input)
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

        public static string ByteArrayToUrlSafeBase64(byte[] byteArray) => Convert.ToBase64String(byteArray)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

        public static string StringToSafeBase64(string str) => ByteArrayToUrlSafeBase64(Encoding.UTF8.GetBytes(str));
    }
}