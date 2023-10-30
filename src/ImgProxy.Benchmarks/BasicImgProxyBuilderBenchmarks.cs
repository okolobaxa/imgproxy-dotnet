using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImgProxy.Benchmarks
{
    /// <summary>
    /// dotnet run -c Release --filter *BasicImgProxyBenchmarks*
    /// </summary>
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net60)]
    public class BasicImgProxyBuilderBenchmarks
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


        private readonly BasicImgProxyBuilder _builder = BasicImgProxyBuilder.New
            .WithEndpoint(Host)
            .WithCredentials(Key, Salt)
            .WithResize(Resize, Width, Height, Gravity, Enlarge)
            .WithFormat(Extension);

        [Benchmark(Baseline = true, Description = "Basic with cached instance")]
        public void BuildInstance()
        {
            var url = _builder.Build(Url);
        }

        [Benchmark(Baseline = false, Description = "Basic without cached instance")]
        public void Build()
        {
            var builder = BasicImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Gravity, Enlarge)
                .WithFormat(Extension);

            var url = builder.Build(Url);
        }
    }
}
