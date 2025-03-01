using Microsoft.Extensions.DependencyInjection;
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Mapping;
using DataLineage.Tracking.Lineage;

namespace DataLineage.Tracking
{
    /// <summary>
    /// Provides extension methods for registering Data Lineage Tracking and Mapping services
    /// into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Registers Data Lineage Tracking and Mapping services into the specified 
        /// <see cref="IServiceCollection"/> for Dependency Injection.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the services should be added.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        /// <remarks>
        /// This method registers:
        /// <list type="bullet">
        /// <item><description><see cref="IDataLineageTracker"/> as a singleton instance of <see cref="DataLineageTracker"/>.</description></item>
        /// <item><description><see cref="IEntityMapper"/> as a singleton instance of <see cref="EntityMapper"/>.</description></item>
        /// </list>
        /// Call this method in your application startup to enable Data Lineage Tracking and Mapping:
        /// <code>
        /// var services = new ServiceCollection();
        /// services.AddDataLineageTracking();
        /// var provider = services.BuildServiceProvider();
        /// </code>
        /// </remarks>
        public static IServiceCollection AddDataLineageTracking(this IServiceCollection services)
        {
            // Register the lineage tracker
            services.AddSingleton<IDataLineageTracker, DataLineageTracker>();

            // Register EntityMapper as the default IEntityMapper
            services.AddSingleton<IEntityMapper, EntityMapper>();

            return services;
        }
    }
}
