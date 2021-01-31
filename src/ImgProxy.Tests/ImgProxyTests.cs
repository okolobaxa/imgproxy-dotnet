using Xunit;

namespace ImgProxy.Tests
{
    public class ImgProxyTests
    {
        const string Key = "943b421c9eb07c830af81030552c86009268de4e532ba2ee2eab8247c6da0881";
        const string Salt = "520f986b998545b4785e0defbc4f3c1203f22de2374a3d53cb7a7fe9fea309c5";
        const string Url = "http://img.example.com/pretty/image.jpg";
        const string Host = "http://cdn.example.com";

        const string Resize = ResizingTypes.Fill;
        const int Width = 300;
        const int Height = 300;
        const string Gravity = GravityTypes.North;
        const bool Enlarge = true;
        const string Extension = Extensions.PNG;

        [Fact]
        public void Build()
        {
            var url = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .Resize(Resize, Width, Height, Gravity, Enlarge)
                .Extension(Extension)
                .Build(Url);

            Assert.Equal("http://cdn.example.com/_PQ4ytCQMMp-1w1m_vP6g8Qb-Q7yF9mwghf6PddqxLw/fill/300/300/no/1/aHR0cDovL2ltZy5leGFtcGxlLmNvbS9wcmV0dHkvaW1hZ2UuanBn.png", url);
        }

        [Fact]
        public void BuildFromInstance()
        {
            var builder = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .Resize(Resize, Width, Height, Gravity, Enlarge)
                .Extension(Extension);

            BasicImgProxyBuilder.Instance = builder;

            var url = BasicImgProxyBuilder.Instance.Build(Url);

            Assert.Equal("http://cdn.example.com/_PQ4ytCQMMp-1w1m_vP6g8Qb-Q7yF9mwghf6PddqxLw/fill/300/300/no/1/aHR0cDovL2ltZy5leGFtcGxlLmNvbS9wcmV0dHkvaW1hZ2UuanBn.png", url);
        }
    }
}
