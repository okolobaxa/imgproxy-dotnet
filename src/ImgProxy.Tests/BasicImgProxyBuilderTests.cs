using Xunit;

namespace ImgProxy.Tests
{
    public class BasicImgProxyBuilderTests
    {
        const string Key = "736563726574";
        const string Salt = "68656C6C6F";
        const string Url = "https://upload.wikimedia.org/wikipedia/ru/2/24/Lenna.png";
        const string Host = "https://cdn.example.com";

        const string Resize = ResizingTypes.Fill;
        const int Width = 300;
        const int Height = 300;
        const string Gravity = GravityTypes.North;
        const bool Enlarge = true;
        const string Extension = Formats.JPG;

        [Fact]
        public void Build()
        {
            var url = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Gravity, Enlarge)
                .WithFormat(Extension)
                .Build(Url);

            Assert.Equal("https://cdn.example.com/5brJRkhAX0yf2HM9qVrCvRF72VNmmDvpPcFJhLXnx5k/fill:300:300:no:1/plain/https://upload.wikimedia.org/wikipedia/ru/2/24/Lenna.png@jpg", url);
        }

        [Fact]
        public void BuildEncoded()
        {
            var url = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Gravity, Enlarge)
                .WithFormat(Extension)
                .Build(Url, true);

            Assert.Equal("https://cdn.example.com/XsvPU1VVCanwDJNEtLH43CnRS1sqJ0LnARho-YltVtQ/fill:300:300:no:1/aHR0cHM6Ly91cGxvYWQud2lraW1lZGlhLm9yZy93aWtpcGVkaWEvcnUvMi8yNC9MZW5uYS5wbmc.jpg", url);
        }

        [Fact]
        public void BuildFromInstance()
        {
            var builder = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Gravity, Enlarge)
                .WithFormat(Extension);

            BasicImgProxyBuilder.Instance = builder;

            var url = BasicImgProxyBuilder.Instance.Build(Url, true);

            Assert.Equal("https://cdn.example.com/XsvPU1VVCanwDJNEtLH43CnRS1sqJ0LnARho-YltVtQ/fill:300:300:no:1/aHR0cHM6Ly91cGxvYWQud2lraW1lZGlhLm9yZy93aWtpcGVkaWEvcnUvMi8yNC9MZW5uYS5wbmc.jpg", url);
        }
    }
}
