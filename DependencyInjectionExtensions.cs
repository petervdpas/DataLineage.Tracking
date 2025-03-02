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
        /// Use this if you need lineage tracking but not entity mapping.
        /// </summary>
        public static IServiceCollection AddDataLineageTracker(this IServiceCollection services)
        {
            services.AddSingleton<IDataLineageTracker, DataLineageTracker>();
            return services;
        }

        /// <summary>
        /// Registers only the Entity Mapper without lineage tracking.
        /// Use this if you need entity mapping but don't need lineage tracking.
        /// </summary>
        public static IServiceCollection AddEntityMapper(this IServiceCollection services)
        {
            services.AddSingleton<IEntityMapper, EntityMapper>();
            return services;
        }

        /// <summary>
        /// Registers both the Data Lineage Tracker and Entity Mapper, ensuring
        /// that the mapper is injected with the lineage tracker.
        /// </summary>
        public static IServiceCollection AddDataLineageTracking(this IServiceCollection services)
        {
            services.AddDataLineageTracker();
            services.AddSingleton<EntityMapper>();
            services.AddSingleton<IEntityMapper>(sp =>
                new EntityMapper(sp.GetRequiredService<IDataLineageTracker>())
            );
            return services;
        }
    }
}
