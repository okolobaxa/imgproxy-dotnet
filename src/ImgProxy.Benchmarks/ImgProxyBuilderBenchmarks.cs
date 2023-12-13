using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImgProxy.Benchmarks
{
    /// <summary>
    ///  dotnet run -c Release --filter *ImgProxyBuilderBenchmarks*
    /// </summary>
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net80)]
    public class ImgProxyBuilderBenchmarks
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

        private readonly ImgProxyOption[] AdditionalOptions = 
        {
            new GravityOption(Gravity),
            new FormatOption(Extension)
        };

        private readonly ImgProxyBuilder _builder = ImgProxyBuilder.New
            .WithEndpoint(Host)
            .WithCredentials(Key, Salt)
            .WithResize(Resize, Width, Height, Enlarge)
            .WithFormat(Extension);

        [Benchmark(Baseline = true, Description = "Without cached instance")]
        public void Build()
        {
            var url = ImgProxyBuilder.New
                .WithEndpoint(Host)
                .WithCredentials(Key, Salt)
                .WithResize(Resize, Width, Height, Enlarge)
                .WithFormat(Extension)
                .Build(Url, false);
        }

        [Benchmark(Baseline = false, Description = "With cached instance")]
        public void BuildFromInstance()
        {
            var url = _builder.Build(Url, false);
        }

        [Benchmark(Baseline = false)]
        public void BuildWithOptions()
        {
            var url = _builder.Build(Url, AdditionalOptions, encode: false);
        }

        [Benchmark(Baseline = false)]
        public void BuildEncoded()
        {
            var url = _builder.Build(Url, encode: true);
        }

        [Benchmark(Baseline = false)]
        public void BuildEncodedWithOptions()
        {
            var url = _builder.Build(Url, AdditionalOptions, encode: true);
        }
    }
}
