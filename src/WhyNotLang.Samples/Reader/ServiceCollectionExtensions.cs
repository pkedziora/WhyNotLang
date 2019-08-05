using Microsoft.Extensions.DependencyInjection;

namespace WhyNotLang.Samples.Reader
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSampleReader(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISampleReader, SampleReader>();
            return serviceCollection;
        }
    }
}
