using Xunit;

namespace ImgProxy.Tests
{
    public class ImgProxyTests2
    {
        const string Key = "a893f0823b38e3f0d753edb970cf5bcfb9935fa86a73a63da9c6cdac06ae561da79462855541c79a9ab4364960970c58f03b81ef712d0178b5937a769d9666ab";
        const string Salt = "ccc346a0e3a9493bf8a6bd1270d3dad2c268401775c0b656754a9242eaa42cdd93a0a5c26b64eb0535cc771560a37e48ff4c00bc23e599e0f06ebee641ebbad3";
        const string Url = "https://oldsaratov.ru/sites/default/files/photos/2018-09/screenshot_1.png";
        const string Host = "https://images.pspr.to";

        const string Resize = ResizingTypes.Fill;
        const int Width = 300;
        const int Height = 400;
        const string Gravity = GravityTypes.Smart;
        const bool Enlarge = false;

        [Fact]
        public void Build()
        {
            var url = ImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, true)
                .WithFormat("jpg")
                .Build(Url);

            Assert.Equal("http://imgproxy.example.com/AfrOrF3gWeDA6VOlDG4TzxMv39O7MXnF4CXpKUwGqRM/preset:sharp/resize:fill:300:400:0/gravity:sm/plain/http://example.com/images/curiosity.jpg@png", url);
        }

        [Fact]
        public void BuildEncoded()
        {
            ImgProxyBuilder.Instance = ImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Enlarge)
                .WithFormat("jpg");

            var url = ImgProxyBuilder.Instance.Build(Url, encoded: true);

            Assert.Equal("https://images.pspr.to/lsANT-dz1_kF4FdE_GFBx7eIQLz1dY4uPAds7iDwivA/resize:fill:300:400:0/gravity:sm/aHR0cHM6Ly9vbGRzYXJhdG92LnJ1L3NpdGVzL2RlZmF1bHQvZmlsZXMvcGhvdG9zLzIwMTgtMDkvc2NyZWVuc2hvdF8xLnBuZw.jpg", url);
        }

        [Fact]
        public void BuildEncodedWithOptions()
        {
            ImgProxyBuilder.Instance = ImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Enlarge);

            var additionalOptions = new ImgProxyOption[]
            {
                new GravityOption(Gravity),
                new FormatOption("jpg")
            };

            var url = ImgProxyBuilder.Instance.Build(Url, additionalOptions, encoded: true);

            Assert.Equal("https://images.pspr.to/0pfSnRKkFbegAV3s4a1a3oteJMF1xM6nz3rQyKPCNQg/resize:fill:300:400:0/gravity:sm/format:jpg/aHR0cHM6Ly9vbGRzYXJhdG92LnJ1L3NpdGVzL2RlZmF1bHQvZmlsZXMvcGhvdG9zLzIwMTgtMDkvc2NyZWVuc2hvdF8xLnBuZw.jpg", url);
        }
    }
}
