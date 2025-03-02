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
        /// Registers only the Data Lineage Tracker in the dependency injection container.
        /// Use this method if you need lineage tracking but do not require entity mapping.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the lineage tracker service should be added.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// services.AddDataLineageTracker();
        /// var provider = services.BuildServiceProvider();
        /// var tracker = provider.GetRequiredService&lt;IDataLineageTracker&gt;();
        /// </code>
        /// </example>
        public static IServiceCollection AddDataLineageTracker(this IServiceCollection services)
        {
            services.AddSingleton<IDataLineageTracker, DataLineageTracker>();
            return services;
        }

        /// <summary>
        /// Registers only the Entity Mapper in the dependency injection container.
        /// Use this method if you need entity mapping without lineage tracking.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the entity mapper service should be added.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// services.AddEntityMapper();
        /// var provider = services.BuildServiceProvider();
        /// var mapper = provider.GetRequiredService&lt;IEntityMapper&gt;();
        /// </code>
        /// </example>
        public static IServiceCollection AddEntityMapper(this IServiceCollection services)
        {
            services.AddSingleton<IEntityMapper, EntityMapper>();
            return services;
        }

        /// <summary>
        /// Registers both the Data Lineage Tracker and the Entity Mapper in the dependency injection container.
        /// Ensures that the mapper is injected with the lineage tracker.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the services should be added.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        /// <example>
        /// <code>
        /// var services = new ServiceCollection();
        /// services.AddDataLineageTracking();
        /// var provider = services.BuildServiceProvider();
        /// var tracker = provider.GetRequiredService&lt;IDataLineageTracker&gt;();
        /// var mapper = provider.GetRequiredService&lt;IEntityMapper&gt;();
        /// </code>
        /// </example>
        public static IServiceCollection AddDataLineageTracking(this IServiceCollection services)
        {
            // Register lineage tracker first
            services.AddDataLineageTracker();

            // Ensure EntityMapper does not create unwanted generic mappings
            services.AddSingleton<EntityMapper>();
            services.AddSingleton<IEntityMapper>(sp =>
            {
                var lineageTracker = sp.GetRequiredService<IDataLineageTracker>();
                return new EntityMapper(lineageTracker);
            });

            return services;
        }
    }
}
