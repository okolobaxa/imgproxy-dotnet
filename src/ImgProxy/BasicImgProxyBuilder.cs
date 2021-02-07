using System;

namespace ImgProxy
{
    /// <summary>
    /// Uses basic URL format that allows the use of all the imgproxy features. User for imgproxy 1.x
    /// </summary>
    public class BasicImgProxyBuilder : BaseBuilder
    {
        /// <summary>
        /// Use to globally cache predefined instance
        /// </summary>
        public static BasicImgProxyBuilder Instance { get; set; }

        /// <summary>
        /// Return new instance of builder
        /// </summary>
        public static BasicImgProxyBuilder New => new BasicImgProxyBuilder();

        private BasicImgProxyBuilder() { }

        /// <summary>
        /// Defines endpoint for the URL
        /// </summary>
        /// <param name="host">Url host</param>
        public BasicImgProxyBuilder WithEndpoint(string host)
        {
            base.WithEndpoint(host);

            return this;
        }

        /// <summary>
        ///  Defines key and salt for signing the URL
        /// </summary>
        /// <param name="key">hex-encoded key</param>
        /// <param name="salt">hex-encoded salt</param>
        public BasicImgProxyBuilder WithCredentials(string key, string salt)
        {
            base.WithCredentials(key, salt);

            return this;
        }

        /// <summary>
        /// Defines how imgproxy will resize the source image
        /// </summary>
        /// <param name="type">Type of resizing. See ResizingTypes for possible values</param>
        /// <param name="width">Width and height parameters define the size of the resulting image in pixels</param>
        /// <param name="height">Width and height parameters define the size of the resulting image in pixels</param>
        /// <param name="gravity">When imgproxy needs to cut some parts of the image, it is guided by the gravity</param>
        /// <param name="enlarge">When set to true, imgproxy will enlarge the image if it is smaller than the given size.</param>
        public BasicImgProxyBuilder WithResize(string type, int width, int height, string gravity, bool enlarge)
        {
            var option = new BasicResizeOption(type, width, height, gravity, enlarge);
            
            AddOption(option);

            return this;
        }

        /// <summary>
        /// Defines format of the resulting image
        /// </summary>
        /// <param name="format">Resulting format. See Formats for possible values</param>
        public BasicImgProxyBuilder WithFormat(string format)
        {
            AddOption(new FormatOption(format));

            return this;
        }

        /// <summary>
        /// Build the URL using defined operations
        /// </summary>
        /// <param name="url">URL of original image</param>
        /// <param name="encode">If true, original image URL will be base64-encoded</param>
        public string Build(string url, bool encode = false)
        {
            return BuildWithFormatAndOptions(url, encode, Options, FormatOption);
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

            return $"{Type}:{Width}:{Height}:{Gravity}:{enlarge}";
        }
    }
}
