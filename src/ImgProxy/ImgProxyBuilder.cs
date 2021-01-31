using System;
using System.Collections.Generic;

namespace ImgProxy
{
    public class ImgProxyBuilder : BaseBuilder
    {
        public static ImgProxyBuilder Instance { get; set; }
        
        public static ImgProxyBuilder New => new ImgProxyBuilder();

        private ImgProxyBuilder() { }
        
        public ImgProxyBuilder WithOptions(params ImgProxyOption[] options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            foreach (var option in options)
            {
                AddOption(option);
            }
            
            return this;
        }
        
        public ImgProxyBuilder WithEndpoint(string host)
        {
            base.WithEndpoint(host);

            return this;
        }

        public ImgProxyBuilder WithCredentials(string key, string salt)
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

        public ImgProxyBuilder WithFormat(string format)
        {   
            AddOption(new FormatOption(format));

            return this;
        }
        
        public ImgProxyBuilder WithPreset(string preset)
        {
            AddOption(new PresetOption(preset));

            return this;
        }

        public string Build(string url, bool encoded = false)
        {
            return BuildWithFormatAndOptions(url, encoded, Options, FormatOption);
        }

        public string Build(string url, IReadOnlyCollection<ImgProxyOption> options, bool encoded = false)
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
                ? BuildWithFormatAndOptions(url, encoded, dict, overrideFormatOption) 
                : BuildWithFormatAndOptions(url, encoded, dict, FormatOption);
        }
    }
}
