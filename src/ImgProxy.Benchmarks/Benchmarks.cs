using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImgProxy.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.CoreRt31)]
    public class ImgProxyBenchmarks
    {
        const string Key = "a893f0823b38e3f0d753edb970cf5bcfb9935fa86a73a63da9c6cdac06ae561da79462855541c79a9ab4364960970c58f03b81ef712d0178b5937a769d9666ab";
        const string Salt = "ccc346a0e3a9493bf8a6bd1270d3dad2c268401775c0b656754a9242eaa42cdd93a0a5c26b64eb0535cc771560a37e48ff4c00bc23e599e0f06ebee641ebbad3";
        const string Url = "https://psprtostoragedev.blob.core.windows.net/full/b00b166c-83f1-4a11-99ae-eb1f8ccf1c9a";
        const string Host = "https://images.pspr.to/";

        const string Resize = ResizingTypes.Fill;
        const int Width = 300;
        const int Height = 300;
        const string Gravity = GravityTypes.North;
        const bool Enlarge = true;
        const string Extension = Extensions.PNG;


        private readonly BasicImgProxyBuilder _builder = BasicImgProxyBuilder.New
            .WithEndpoint(Host)
            .WithCredentials(Key, Salt)
            .Resize(Resize, Width, Height, Gravity, Enlarge)
            .Extension(Extension);

        [Benchmark]
        public void BuildInstance()
        {
            var url = _builder.Build(Url);
        }

        [Benchmark]
        public void Build()
        {
            var builder = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .Resize(Resize, Width, Height, Gravity, Enlarge)
                .Extension(Extension);

            var url = builder.Build(Url);
        }
    }
}
