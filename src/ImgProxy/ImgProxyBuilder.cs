using System;
using System.Collections.Generic;

namespace ImgProxy
{
    /// <summary>
    /// Uses advanced URL format that allows the use of all the imgproxy features. User for imgproxy 2.x
    /// </summary>
    public class ImgProxyBuilder : BaseBuilder
    {
        /// <summary>
        /// Use to globally cache predefined instance
        /// </summary>
        public static ImgProxyBuilder Instance { get; set; }

        /// <summary>
        /// Return new instance of builder
        /// </summary>
        public static ImgProxyBuilder New => new ImgProxyBuilder();

        private ImgProxyBuilder() { }

        /// <summary>
        /// Defines endpoint for the URL
        /// </summary>
        /// <param name="host">Url host</param>
        public new ImgProxyBuilder WithEndpoint(string host)
        {
            base.WithEndpoint(host);

            return this;
        }

        /// <summary>
        ///  Defines key and salt for signing the URL
        /// </summary>
        /// <param name="key">hex-encoded key</param>
        /// <param name="salt">hex-encoded salt</param>
        public new ImgProxyBuilder WithCredentials(string key, string salt)
        {
            base.WithCredentials(key, salt);

            return this;
        }
        
        /// <summary>
        /// Meta-option that defines the resizing type, width, height, enlarge, and extend.
        /// </summary>
        public ImgProxyBuilder WithResize(string resize, int width, int height, bool enlarge)
        {
            AddOption(new ResizeOption(resize, width, height, enlarge));

            return this;
        }
        
        /// <summary>
        /// Adds 'raw' processing option
        /// </summary>
        public ImgProxyBuilder WithRaw()
        {
            AddOption(new RawOption());

            return this;
        }

        /// <summary>
        /// Defines format of the resulting image
        /// </summary>
        /// <param name="format">Resulting format. See Formats for possible values</param>
        public ImgProxyBuilder WithFormat(string format)
        {   
            AddOption(new FormatOption(format));

            return this;
        }

        /// <summary>
        /// Defines preset to be used for generating an image
        /// </summary>
        /// <param name="preset">Preset name</param>
        public ImgProxyBuilder WithPreset(string preset)
        {
            AddOption(new PresetOption(preset));

            return this;
        }

        /// <summary>
        /// Defines options to use with other defined options.
        /// </summary>
        /// <param name="options">Options to be used in URL generation</param>
        public ImgProxyBuilder WithOptions(params ImgProxyOption[] options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            foreach (var option in options)
            {
                AddOption(option);
            }
            
            return this;
        }

        /// <summary>
        /// Build the URL using defined options
        /// </summary>
        /// <param name="url">URL of original image</param>
        /// <param name="encode">If true, original image URL will be base64-encoded</param>
        public string Build(string url, bool encode = false)
        {
            return BuildWithFormatAndOptions(url, encode, Options, FormatOption);
        }

        /// <summary>
        ///  Build the URL using defined options and additional passed options
        /// </summary>
        /// <param name="url">URL of original image</param>
        /// <param name="options">Additional options to be used in URL generation</param>
        /// <param name="encode">If true, original image URL will be base64-encoded</param>
        /// <returns></returns>
        public string Build(string url, IReadOnlyCollection<ImgProxyOption> options, bool encode = false)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            if (options == null) throw new ArgumentNullException(nameof(options));

            FormatOption overrideFormatOption = null;

            var dict = new Dictionary<string, ImgProxyOption>(Options);
            foreach (var option in options)
            {
                if (option is FormatOption formatOption)
                {
                    overrideFormatOption = formatOption;
                }
                else
                {
                    dict[option.GetType().Name] = option;
                }
            }

            return overrideFormatOption != null 
                ? BuildWithFormatAndOptions(url, encode, dict, overrideFormatOption) 
                : BuildWithFormatAndOptions(url, encode, dict, FormatOption);
        }
    }
}
