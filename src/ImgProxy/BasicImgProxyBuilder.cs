using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ImgProxy
{
    public class BasicImgProxyBuilder : BaseBuilder
    {
        public static BasicImgProxyBuilder New => new BasicImgProxyBuilder();

        public static BasicImgProxyBuilder Instance { get; set; }
        
        private BasicImgProxyBuilder() { }
        
        public BasicImgProxyBuilder WithEndpoint(string host)
        {
            base.WithEndpoint(host);

            return this;
        }

        public BasicImgProxyBuilder WithCredentials(string key, string salt)
        {
            base.WithCredentials(key, salt);

            return this;
        }
        
        public BasicImgProxyBuilder Resize(string type, int width, int height, string gravity, bool enlarge)
        {
            var option = new BasicResizeOption(type, width, height, gravity, enlarge);
            
            AddOption(option);

            return this;
        }

        public BasicImgProxyBuilder Extension(string extension)
        {
            var formatOption = new FormatOption(extension);
            
            AddOption(formatOption);

            return this;
        }

        public string Build(string url, bool encode = false)
        {
            return BuildWithFormatAndOptions(url, encode, new Dictionary<string, ImgProxyOption>(), null);
        }
    }

    internal class BasicResizeOption : ImgProxyOption
    {
        private string Type { get; }
        private int Width { get; }
        private int Height { get; }
        private string Gravity { get; }
        private bool Enlarge { get; }

        public BasicResizeOption(string type, int width, int height, string gravity, bool enlarge = false)
        {
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));

            Type = type;
            Width = width;
            Height = height;
            Gravity = gravity;
            Enlarge = enlarge;
        }

        public override string ToString()
        {
            var enlarge = Enlarge ? BoolStrings.True : BoolStrings.False;

            return $"{Type}:{Width}:{Height}:{enlarge}";
        }
    }
}
