# imgproxy-dotnet

<img align="right" width="200" height="200" title="imgproxy logo"
     src="https://cdn.rawgit.com/DarthSim/imgproxy/master/logo.svg">

![NuGet](https://img.shields.io/nuget/v/LambdaExpressionBuilder.svg)

**[imgproxy](https://github.com/imgproxy/imgproxy)** is a fast and secure standalone server for resizing and converting remote images. The main principles of imgproxy are simplicity, speed, and security. It is a Go application, ready to be installed and used in any Unix environment—also ready to be containerized using Docker.

imgproxy can be used to provide a fast and secure way to replace all the image resizing code of your web application (like calling ImageMagick or GraphicsMagick, or using libraries), while also being able to resize everything on the fly, fast and easy. imgproxy is also indispensable when handling lots of image resizing, especially when images come from a remote source.

**imgproxy-dotnet** is a small C# lib, that helps to build correct URLs for imgproxy.

# Features
* Support imgproxy 1.x URL format
* Support imgproxy 2.x URL format (recomended)

## Installation

Using .NET CLI

```
dotnet add package ImgProxy --version 2.1.0
```

or using Package Manager

```
Install-Package ImgProxy -Version 2.1.0
```

# Usage
Create new builder for each call
```CSharp

const string host = "https://cdn.example.com";
const string key = "736563726574";
const string salt = "68656C6C6F";
const string source = "https://upload.wikimedia.org/wikipedia/ru/2/24/Lenna.png";

var bulder = ImgProxyBuilder.New
                .WithEndpoint(host)
                .WithCredentials(key, salt)
                .WithResize(ResizingTypes.Fill, 300, 400, true)
                .WithFormat(Formats.JPG);

var url = bulder.Build(source, encode: false);                
```

Or cache global-configured instance, e.g. in Sturtup.cs   
```CSharp

var bulder = ImgProxyBuilder.New
                .WithEndpoint(host)
                .WithCredentials(key, salt)
                .WithResize(ResizingTypes.Fill, 300, 400, true)
                .WithFormat(Formats.JPG);
            
ImgProxyBuilder.Instance = bulder;
                
var url =  ImgProxyBuilder.Instance.Build(source, encode: false);         
```

Also you can pass additional options in time of build. Cached builder will not be affected. In case of duplicate options (defined in bulder and passed via param), options from params will be used.
```CSharp
var additionalOptions = new ImgProxyOption[]
{
      new GravityOption(Gravity),
      new FormatOption(Formats.JPG)
};

var url = bulder.Build(Url, additionalOptions, encode: false);            
```

## Supported options
Pass this options to .WithOptions ot .Build methods:

  * ResizeOption
  * ResizingTypeOption
  * ResizingAlgorithmOption
  * SizeOption
  * WidthOption
  * HeightOption
  * DprOption
  * RotateOption
  * QualityOption
  * MaxBytesOption
  * BackgroundOption
  * BackgroundAlphaOption
  * AdjustOption
  * BrightnessOption
  * ContrastOption
  * SaturationOption
  * BlurOption
  * SharpenOption
  * PixelateOption
  * UnsharpeningOption
  * EnlargeOption
  * ExtendOption
  * CropOption
  * PaddingOption
  * TrimOption
  * GravityOption
  * JpegOption
  * PngOption
  * GifOption
  * WatermarkOption
  * WatermarkUrlOption
  * PageOption
  * VideoThumbnailSecondOption
  * StyleOption
  * CacheBusterOption
  * StripMetadataOption
  * StripColorProfileOption
  * FormatOption
  * AutoRotateOption
  * FilenameOption
  * PresetOption
  
  ## Contributing

Bug reports and pull requests are welcome on GitHub at https://github.com/okolobaxa/imgproxy-dotnet.

If you are having any problems with image processing of imgproxy itself, be sure to visit https://github.com/imgproxy/imgproxy first and check out the docs at https://github.com/imgproxy/imgproxy/blob/master/docs/.

## License

The lib is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).
