using System;
using System.Collections.Generic;

namespace ImgProxy
{
    public abstract class ImgProxyOption { }

    /// <summary>
    /// Meta-option that defines the resizing type, width, height, enlarge, and extend.
    /// </summary>
    public class ResizeOption : ImgProxyOption
    {
        public string Type { get; }
        public int Width { get; }
        public int Height { get; }
        public bool Enlarge { get; }
        public bool Extend { get; }

        public ResizeOption(string type, int width, int height, bool enlarge = false, bool extend = false)
        {
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            Type = type;
            Width = width;
            Height = height;
            Enlarge = enlarge;
            Extend = extend;
        }

        public override string ToString()
        {
            var enlarge = Enlarge ? BoolStrings.True : BoolStrings.False;
            var extend = Extend ? BoolStrings.True : BoolStrings.False;

            return $"resize:{Type}:{Width}:{Height}:{enlarge}:{extend}";
        }
    }

    /// <summary>
    /// Defines how imgproxy will resize the source image. Supported resizing types are:
    ///fit: resizes the image while keeping aspect ratio to fit given size;
    ///fill: resizes the image while keeping aspect ratio to fill given size and cropping projecting parts;
    ///auto: if both source and resulting dimensions have the same orientation (portrait or landscape), imgproxy will use fill. Otherwise, it will use fit.
    /// </summary>
    public class ResizingTypeOption : ImgProxyOption
    {
        public string Type { get; }

        public ResizingTypeOption(string type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"resizing_type:{Type}";
        }
    }

    /// <summary>
    /// Defines the algorithm that imgproxy will use for resizing. Supported algorithms are nearest, linear, cubic, lanczos2, and lanczos3.
    /// </summary>
    public class ResizingAlgorithmOption : ImgProxyOption
    {
        public string Algorithm { get; }

        public ResizingAlgorithmOption(string algorithm)
        {
            Algorithm = algorithm;
        }

        public override string ToString()
        {
            return $"resizing_algorithm:{Algorithm}";
        }
    }

    /// <summary>
    /// Meta-option that defines the width, height, enlarge, and extend. 
    /// </summary>
    public class SizeOption : ImgProxyOption
    {
        public int Width { get; }
        public int Height { get; }
        public bool Enlarge { get; }
        public bool Extend { get; }

        public SizeOption(int width, int height, bool enlarge = false, bool extend = false)
        {
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            Width = width;
            Height = height;
            Enlarge = enlarge;
            Extend = extend;
        }

        public override string ToString()
        {
            var enlarge = Enlarge ? BoolStrings.True : BoolStrings.False;
            var extend = Extend ? BoolStrings.True : BoolStrings.False;

            return $"size:{Width}:{Height}:{enlarge}:{extend}";
        }
    }

    /// <summary>
    /// Defines the width of the resulting image. When set to 0, imgproxy will calculate the resulting width using the defined height and source aspect ratio.
    /// </summary>
    public class WidthOption : ImgProxyOption
    {
        public int Width { get; }

        public WidthOption(int width)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));

            Width = width;
        }

        public override string ToString()
        {
            return $"width:{Width}";
        }
    }

    /// <summary>
    /// When set, imgproxy will multiply the image dimensions according to this factor for HiDPI (Retina) devices. The value must be greater than 0.
    /// </summary>
    public class DprOption : ImgProxyOption
    {
        public int Dpr { get; }

        public DprOption(int dpr)
        {
            if (dpr <= 0) throw new ArgumentOutOfRangeException(nameof(dpr));

            Dpr = dpr;
        }

        public override string ToString()
        {
            return $"dpr:{Dpr}";
        }
    }

    /// <summary>
    /// Rotates the image on the specified angle. The orientation from the image metadata is applied before the rotation unless autorotation is disabled.
    /// </summary>
    public class RotateOption : ImgProxyOption
    {
        public int Angel { get; }

        public RotateOption(int angel)
        {
            if (angel < 0) throw new ArgumentOutOfRangeException(nameof(angel));
            if (angel > 0 && angel % 90 != 0) throw new ArgumentOutOfRangeException(nameof(angel));

            Angel = angel;
        }

        public override string ToString()
        {
            return $"rotate:{Angel}";
        }
    }

    /// <summary>
    /// Redefines quality of the resulting image, percentage.
    /// </summary>
    public class QualityOption : ImgProxyOption
    {
        public int Quality { get; }

        public QualityOption(int quality)
        {
            if (quality < 0) throw new ArgumentOutOfRangeException(nameof(quality));

            Quality = quality;
        }

        public override string ToString()
        {
            return $"quality:{Quality}";
        }
    }

    /// <summary>
    /// When set, imgproxy automatically degrades the quality of the image until the image is under the specified amount of bytes.
    /// </summary>
    public class MaxBytesOption : ImgProxyOption
    {
        public long MaxBytes { get; }

        public MaxBytesOption(long maxBytes)
        {
            if (maxBytes <= 0) throw new ArgumentOutOfRangeException(nameof(maxBytes));

            MaxBytes = maxBytes;
        }

        public override string ToString()
        {
            return $"max_bytes:{MaxBytes}";
        }
    }

    /// <summary>
    /// Adds alpha channel to background. alpha is a positive floating point number between 0 and 1.
    /// </summary>
    public class BackgroundAlphaOption : ImgProxyOption
    {
        public float Alpha { get; }

        public BackgroundAlphaOption(float alpha)
        {
            if (alpha < 0) throw new ArgumentOutOfRangeException(nameof(alpha));

            Alpha = alpha;
        }

        public override string ToString()
        {
            return $"bga:{Alpha}";
        }
    }

    /// <summary>
    /// When set, imgproxy will fill the resulting image background with the specified color
    /// </summary>
    public class BackgroundOption : ImgProxyOption
    {
        public string Color { get; }
        public byte? R { get; }
        public byte? G { get; }
        public byte? B { get; }

        public BackgroundOption(string color)
        {
            Color = color;
        }

        public BackgroundOption(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Color)
                ? $"background:{Color}"
                : $"background:{R}:{G}:{B}";
        }
    }

    /// <summary>
    /// Meta-option that defines the brightness, contrast, and saturation.
    /// </summary>
    public class AdjustOption : ImgProxyOption
    {
        public short Brightness { get; }
        public short Contrast { get; }
        public short Saturation { get; }

        public AdjustOption(short brightness, short contrast, short saturation)
        {
            if (brightness < -255 || brightness > 255) throw new ArgumentOutOfRangeException(nameof(brightness));
            if (contrast < -255 || contrast > 255) throw new ArgumentOutOfRangeException(nameof(contrast));
            if (saturation < -255 || saturation > 255) throw new ArgumentOutOfRangeException(nameof(saturation));

            Brightness = brightness;
            Contrast = contrast;
            Saturation = saturation;
        }

        public override string ToString()
        {
            return $"adjust:{Brightness}:{Contrast}:{Saturation}";
        }
    }

    /// <summary>
    /// When set, imgproxy will adjust brightness of the resulting image.
    /// </summary>
    public class BrightnessOption : ImgProxyOption
    {
        public short Brightness { get; }

        public BrightnessOption(short brightness)
        {
            if (brightness < -255 || brightness > 255) throw new ArgumentOutOfRangeException(nameof(brightness));

            Brightness = brightness;
        }

        public override string ToString()
        {
            return $"brightness:{Brightness}";
        }
    }

    /// <summary>
    /// When set, imgproxy will adjust contrast of the resulting image.
    /// </summary>
    public class ContrastOption : ImgProxyOption
    {
        public short Contrast { get; }

        public ContrastOption(short contrast)
        {
            if (contrast < -255 || contrast > 255) throw new ArgumentOutOfRangeException(nameof(contrast));

            Contrast = contrast;
        }

        public override string ToString()
        {
            return $"contrast:{Contrast}";
        }
    }

    /// <summary>
    /// When set, imgproxy will adjust saturation of the resulting image.
    /// </summary>
    public class SaturationOption : ImgProxyOption
    {
        public short Saturation { get; }

        public SaturationOption(short saturation)
        {
            if (saturation < -255 || saturation > 255) throw new ArgumentOutOfRangeException(nameof(saturation));

            Saturation = saturation;
        }

        public override string ToString()
        {
            return $"saturation:{Saturation}";
        }
    }

    /// <summary>
    /// When set, imgproxy will apply the gaussian blur filter to the resulting image.
    /// </summary>
    public class BlurOption : ImgProxyOption
    {
        public float Sigma { get; }

        public BlurOption(float sigma)
        {
            if (sigma < 0) throw new ArgumentOutOfRangeException(nameof(sigma));

            Sigma = Sigma;
        }

        public override string ToString()
        {
            return $"blur:{Sigma}";
        }
    }

    /// <summary>
    /// When set, imgproxy will apply the sharpen filter to the resulting image.
    /// </summary>
    public class SharpenOption : ImgProxyOption
    {
        public float Sigma { get; }

        public SharpenOption(float sigma)
        {
            if (sigma < 0) throw new ArgumentOutOfRangeException(nameof(sigma));

            Sigma = Sigma;
        }

        public override string ToString()
        {
            return $"sharpen:{Sigma}";
        }
    }

    /// <summary>
    /// When set, imgproxy will apply the pixelate filter to the resulting image.
    /// </summary>
    public class PixelateOption : ImgProxyOption
    {
        public float Size { get; }

        public PixelateOption(float size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
        }

        public override string ToString()
        {
            return $"pixelate:{Size}";
        }
    }

    /// <summary>
    /// Allows redefining unsharpening options.
    /// </summary>
    public class UnsharpeningOption : ImgProxyOption
    {
        public string Mode { get; }

        public float Weight { get; }

        public float Dividor { get; }

        public UnsharpeningOption(string mode, float weight, float dividor)
        {
            if (weight < 0) throw new ArgumentOutOfRangeException(nameof(weight));
            if (dividor < 0) throw new ArgumentOutOfRangeException(nameof(dividor));

            Mode = mode;
            Weight = weight;
            Dividor = dividor;
        }

        public override string ToString()
        {
            return $"unsharpening:{Mode}:{Weight}:{Dividor}";
        }
    }

    /// <summary>
    /// Defines the height of the resulting image. When set to 0, imgproxy will calculate resulting height using the defined width and source aspect ratio.
    /// </summary>
    public class HeightOption : ImgProxyOption
    {
        public int Height { get; }

        public HeightOption(int height)
        {
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));

            Height = height;
        }

        public override string ToString()
        {
            return $"height:{Height}";
        }
    }

    /// <summary>
    /// When set to true, imgproxy will enlarge the image if it is smaller than the given size.
    /// </summary>
    public class EnlargeOption : ImgProxyOption
    {
        public bool Enlarge { get; }

        public EnlargeOption(bool enlarge)
        {
            Enlarge = enlarge;
        }

        public override string ToString()
        {
            var enlarge = Enlarge ? BoolStrings.True : BoolStrings.False;

            return $"enlarge:{enlarge}";
        }
    }

    /// <summary>
    /// When extend is set to true, imgproxy will extend the image if it is smaller than the given size.
    /// </summary>
    public class ExtendOption : ImgProxyOption
    {
        public bool Extend { get; }

        public GravityOption GravityOption { get; }

        public ExtendOption(bool extend, GravityOption option = null)
        {
            Extend = extend;
            GravityOption = option;

            if (option != null && option.Type == GravityTypes.Smart)
            {
                throw new ArgumentException(nameof(option));
            }
        }

        public override string ToString()
        {
            var extend = Extend ? BoolStrings.True : BoolStrings.False;

            return GravityOption == null
                ? $"extend:{extend}"
                : $"extend:{extend}:{GravityOption}";
        }
    }

    /// <summary>
    /// Defines an area of the image to be processed (crop before resize).
    /// </summary>
    public class CropOption : ImgProxyOption
    {
        public float Width { get; }

        public float Height { get; }

        public GravityOption GravityOption { get; }

        public CropOption(float width, float height, GravityOption option = null)
        {
            Width = width;
            Height = height;
            GravityOption = option;
        }

        public override string ToString()
        {
            return GravityOption == null
                ? $"crop:{Width}:{Height}"
                : $"crop:{Width}:{Height}:{GravityOption}";
        }
    }

    /// <summary>
    /// Defines padding size in css manner. All arguments are optional but at least one dimension must be set. Padded space is filled according to background option.
    /// </summary>
    public class PaddingOption : ImgProxyOption
    {
        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public int Left { get; }

        public PaddingOption(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public override string ToString()
        {
            return $"padding:{Top}:{Right}:{Bottom}:{Left}";
        }
    }

    /// <summary>
    /// Removes surrounding background.
    /// </summary>
    public class TrimOption : ImgProxyOption
    {
        public float Threshold { get; }

        public string Color { get; }

        public bool EqualHorizontal { get; }

        public bool EqualVertical { get; }

        public TrimOption(float threshold, string color = null, bool equalHorizontal = false,
            bool equalVertical = false)
        {
            Threshold = threshold;
            Color = color;
            EqualHorizontal = equalHorizontal;
            EqualVertical = equalHorizontal;
        }

        public override string ToString()
        {
            return $"trim:{Threshold}:{Color}:{EqualHorizontal}:{EqualVertical}";
        }
    }

    /// <summary>
    /// When imgproxy needs to cut some parts of the image, it is guided by the gravity.
    /// </summary>
    public class GravityOption : ImgProxyOption
    {
        public string Type { get; }

        public float? OffsetX { get; }

        public float? OffsetY { get; }

        public GravityOption(string type, float? offsetX = null, float? offsetY = null)
        {
            Type = type;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        /// <summary>
        /// libvips detects the most “interesting” section of the image and considers it as the center of the resulting image
        /// </summary>
        public static GravityOption Smart() => new GravityOption(GravityTypes.Smart);

        /// <summary>
        /// Focus point gravity. x and y are floating point numbers between 0 and 1 that define the coordinates of the center of the resulting image. Treat 0 and 1 as right/left for x and top/bottom for y.
        /// </summary>
        public static GravityOption FocusPoint(float x, float y)
        {
            if (x < 0 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y > 1) throw new ArgumentOutOfRangeException(nameof(x));

            return new GravityOption(GravityTypes.FocusPoint, x, y);
        }

        public override string ToString()
        {
            return OffsetX.HasValue || OffsetY.HasValue
                ? $"gravity:{Type}:{OffsetX}:{OffsetY}"
                : $"gravity:{Type}";
        }
    }

    /// <summary>
    /// Puts watermark on the processed image.
    /// </summary>
    public class WatermarkOption : ImgProxyOption
    {
        public float Opacity { get; }

        public string Position { get; }

        public float? OffsetX { get; }

        public float? OffsetY { get; }

        public float? Scale { get; }

        public WatermarkOption(float opacity, string position = null, float? offsetX = null, float? offsetY = null,
            float scale = 0)
        {
            Opacity = opacity;
            Position = position;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Scale = scale;
        }

        public override string ToString()
        {
            return $"watermark:{Opacity}:{Position}:{OffsetX}:{OffsetY}:{Scale}";
        }
    }
    
    /// <summary>
    /// Allows redefining JPEG saving options.
    /// </summary>
    public class JpegOption : ImgProxyOption
    {
        public bool Progressive { get; }

        public bool NoSubsample { get; }

        public bool TrellisQuant { get; }

        public bool OvershootDeringing { get; }

        public bool OptimizeScans { get; }
        
        public ushort QuantTable { get; }

        public JpegOption(bool progressive, bool noSubsample, bool trellisQuant, bool overshootDeringing, bool optimizeScans, ushort quantTable)
        {
            Progressive = progressive;
            NoSubsample = noSubsample;
            TrellisQuant = trellisQuant;
            OvershootDeringing = overshootDeringing;
            OptimizeScans = optimizeScans;
            QuantTable = quantTable;

            if (quantTable < 0 || quantTable > 8) throw new ArgumentOutOfRangeException(nameof(quantTable));
        }

        public override string ToString()
        {
            var noSubsample = NoSubsample ? BoolStrings.True : BoolStrings.False;
            var trellisQuant = TrellisQuant ? BoolStrings.True : BoolStrings.False;
            var overshootDeringing = OvershootDeringing ? BoolStrings.True : BoolStrings.False;
            var optimizeScans = OptimizeScans ? BoolStrings.True : BoolStrings.False;
            
            return $"jpeg_options:{Progressive}:{noSubsample}:{trellisQuant}:{overshootDeringing}:{optimizeScans}:{QuantTable}";
        }
    }
    
    /// <summary>
    /// Allows redefining PNG saving options.
    /// </summary>
    public class PngOption : ImgProxyOption
    {
        public bool Interlaced { get; }

        public bool Quantize { get; }

        public byte QuantizationСolors { get; }

        public PngOption(bool interlaced, bool quantize, byte quantizationСolors)
        {
            Interlaced = interlaced;
            Quantize = quantize;
            QuantizationСolors = quantizationСolors;

            if (QuantizationСolors < 1 || QuantizationСolors > 255) throw new ArgumentOutOfRangeException(nameof(quantizationСolors));
        }

        public override string ToString()
        {
            var interlaced = Interlaced ? BoolStrings.True : BoolStrings.False;
            var quantize = Quantize ? BoolStrings.True : BoolStrings.False;
            
            return $"png_options:{interlaced}:{quantize}:{QuantizationСolors}";
        }
    }
    
    /// <summary>
    /// Allows redefining GIF saving options
    /// </summary>
    public class GifOption : ImgProxyOption
    {
        public bool OptimizeFrames { get; }

        public bool OptimizeTransparency { get; }

        public GifOption(bool optimizeFrames, bool optimizeTransparency)
        {
            OptimizeFrames = optimizeFrames;
            OptimizeTransparency = optimizeTransparency; 
        }

        public override string ToString()
        {
            var optimizeFrames = OptimizeFrames ? BoolStrings.True : BoolStrings.False;
            var optimizeTransparency = OptimizeTransparency ? BoolStrings.True : BoolStrings.False;
            
            return $"gif_options:{optimizeFrames}:{optimizeTransparency}";
        }
    }

    /// <summary>
    /// When set, imgproxy will use the image from the specified URL as a watermark.
    /// </summary>
    public class WatermarkUrlOption : ImgProxyOption
    {
        public string Url { get; }

        public WatermarkUrlOption(string url)
        {
            Url = HexHelper.StringToSafeBase64(url);
        }

        public override string ToString()
        {
            return $"watermark_url:{Url}";
        }
    }
    
    /// <summary>
    /// When source image supports pagination (PDF, TIFF) or animation (GIF, WebP), this option allows specifying the page to use
    /// </summary>
    public class PageOption : ImgProxyOption
    {
        public int Page { get; }

        public PageOption(int page)
        {
            Page = page;
        }

        public override string ToString()
        {
            return $"page:{Page}";
        }
    }
    
    /// <summary>
    /// Allows redefining IMGPROXY_VIDEO_THUMBNAIL_SECOND config.
    /// </summary>
    public class VideoThumbnailSecondOption : ImgProxyOption
    {
        public int Seconds { get; }

        public VideoThumbnailSecondOption(int seconds)
        {
            Seconds = seconds;
        }

        public override string ToString()
        {
            return $"video_thumbnail_second:{Seconds}";
        }
    }
    
    /// <summary>
    /// When set, imgproxy will prepend <style> node with provided content to the <svg> node of source SVG image.
    /// </summary>
    public class StyleOption : ImgProxyOption
    {
        public string Style { get; }

        public StyleOption(string url)
        {
            Style = HexHelper.StringToSafeBase64(url);
        }

        public override string ToString()
        {
            return $"style:{Style}";
        }
    }
    
    /// <summary>
    /// Cache buster doesn’t affect image processing but it’s changing allows to bypass CDN, proxy server and browser cache. Useful when you have changed some things that are not reflected in the URL like image quality settings, presets or watermark data.
    /// </summary>
    public class CacheBusterOption : ImgProxyOption
    {
        public string Token { get; }

        public CacheBusterOption(string token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"cachebuster:{Token}";
        }
    }

    /// <summary>
    /// When set to true, imgproxy will strip the metadata (EXIF, IPTC, etc.) on JPEG and WebP output images.
    /// </summary>
    public class StripMetadataOption : ImgProxyOption
    {
        public bool StripMetadata { get; }

        public StripMetadataOption(bool stripMetadata)
        {
            StripMetadata = stripMetadata;
        }

        public override string ToString()
        {
            var strip = StripMetadata ? BoolStrings.True : BoolStrings.False;
            return $"strip_metadata:{strip}";
        }
    }

    /// <summary>
    /// When set to true, imgproxy will transform the embedded color profile (ICC) to sRGB and remove it from the image.
    /// </summary>
    public class StripColorProfileOption : ImgProxyOption
    {
        public bool StripColorProfile { get; }

        public StripColorProfileOption(bool stripColorProfile)
        {
            StripColorProfile = stripColorProfile;
        }

        public override string ToString()
        {
            var strip = StripColorProfile ? BoolStrings.True : BoolStrings.False;
            return $"strip_color_profile:{strip}";
        }
    }

    /// <summary>
    /// Specifies the resulting image format
    /// </summary>
    public class FormatOption : ImgProxyOption
    {
        public string Format { get; }

        public FormatOption(string format)
        {
            Format = format;
        }

        public override string ToString()
        {
            return $"format:{Format}";
        }
    }

    /// <summary>
    /// When set to true, imgproxy will automatically rotate images based onon the EXIF Orientation parameter (if available in the image meta data)
    /// </summary>
    public class AutoRotateOption : ImgProxyOption
    {
        public bool AutoRotate { get; }

        public AutoRotateOption(bool autoRotate)
        {
            AutoRotate = autoRotate;
        }

        public override string ToString()
        {
            return $"auto_rotate:{AutoRotate}";
        }
    }

    /// <summary>
    /// Defines a filename for Content-Disposition header.
    /// </summary>
    public class FilenameOption : ImgProxyOption
    {
        public string Filename { get; }

        public FilenameOption(string filename)
        {
            Filename = filename;
        }

        public override string ToString()
        {
            return $"filename:{Filename}";
        }
    }

    /// <summary>
    /// Defines a list of presets to be used by imgproxy. Feel free to use as many presets in a single URL as you need.
    /// </summary>
    public class PresetOption : ImgProxyOption
    {
        public IReadOnlyCollection<string> Presets { get; }

        public PresetOption(params string[] presets)
        {
            Presets = presets;
        }

        public override string ToString()
        {
            return $"preset:{string.Join(":", Presets)}";
        }
    }
}