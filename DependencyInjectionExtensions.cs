using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Mapping;
using DataLineage.Tracking.Lineage;
using DataLineage.Tracking.Configuration;
using System;

namespace DataLineage.Tracking
{
    /// <summary>
    /// Provides extension methods for registering Data Lineage Tracking and Mapping services
    /// into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Registers Data Lineage Tracking and Mapping services in the dependency injection container.
        /// Allows configuration through <see cref="DataLineageOptions"/>.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the services should be added.
        /// </param>
        /// <param name="configureOptions">
        /// A delegate to configure <see cref="DataLineageOptions"/>. Optional.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        public static IServiceCollection AddDataLineageTracking(
            this IServiceCollection services,
            Action<DataLineageOptions>? configureOptions = null)
        {
            // Register default configuration using Microsoft's Options pattern
            services.AddOptions<DataLineageOptions>()
                    .Configure(configureOptions ?? (_ => { })) // Apply user-defined options if provided
                    .Services
                    .AddSingleton<IConfigureOptions<DataLineageOptions>, ConfigureDataLineageOptions>();

            // Register lineage tracker
            services.AddSingleton<IDataLineageTracker, DataLineageTracker>();

            // Register EntityMapper with options
            services.AddSingleton<IEntityMapper, EntityMapper>();

            return services;
        }
    }
}
