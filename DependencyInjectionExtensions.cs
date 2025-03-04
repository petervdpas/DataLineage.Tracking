using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Mapping;
using DataLineage.Tracking.Lineage;
using DataLineage.Tracking.Configuration;
using System;
using DataLineage.Tracking.Sinks;
using System.Reflection;
using System.Linq;

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
        /// Allows configuration through <see cref="DataLineageOptions"/> and optional sinks.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the services should be added.
        /// </param>
        /// <param name="assembly">The assembly containing mappers to register.</param>
        /// <param name="configureOptions">
        /// A delegate to configure <see cref="DataLineageOptions"/>. Optional.
        /// </param>
        /// <param name="sinks">
        /// Optional sinks to store lineage data (e.g., file sink). Defaults to an in-memory tracker if empty.
        /// </param>
        /// <returns>
        /// The modified <see cref="IServiceCollection"/> for method chaining.
        /// </returns>
        public static IServiceCollection AddDataLineageTracking(
            this IServiceCollection services,
            Assembly assembly,
            Action<DataLineageOptions>? configureOptions = null,
            params ILineageSink[] sinks)
        {
            // Register configuration
            services.AddOptions<DataLineageOptions>()
                    .Configure(configureOptions ?? (_ => { }))
                    .Services
                    .AddSingleton<IConfigureOptions<DataLineageOptions>, ConfigureDataLineageOptions>();

            // Register lineage tracker with optional sinks
            services.AddSingleton<IDataLineageTracker>(sp => new DataLineageTracker(sinks));

            var implTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.BaseType != null)
                .ToList();

            foreach (var implementationType in implTypes)
            {
                var baseType = implementationType.BaseType;
                
                // Ensure baseType is generic before calling GetGenericTypeDefinition() WrapperInterface??
                if (!baseType!.IsGenericType) continue;
                
                var baseGenericType = baseType.GetGenericTypeDefinition();

                if (baseGenericType == typeof(TrackableEntityMapper<,>))
                {
                    var interfaceType = implementationType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTracker<,>));

                    if (interfaceType != null)
                    {
                        services.AddSingleton(interfaceType, implementationType);
                    }
                }
            }

            return services;
        }
    }
}
