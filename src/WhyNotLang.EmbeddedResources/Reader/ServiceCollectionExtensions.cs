using Microsoft.Extensions.DependencyInjection;

namespace WhyNotLang.EmbeddedResources.Reader
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSampleReader(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IResourceReader, ResourceReader>();
            return serviceCollection;
        }
    }
}
