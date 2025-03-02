using Microsoft.Extensions.Options;

namespace DataLineage.Tracking.Configuration
{
    /// <summary>
    /// Provides default configuration values for <see cref="DataLineageOptions"/>.
    /// Implements <see cref="IConfigureOptions{TOptions}"/> to allow dependency injection.
    /// </summary>
    public class ConfigureDataLineageOptions : IConfigureOptions<DataLineageOptions>
    {
        /// <summary>
        /// Configures the default settings for Data Lineage options.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(DataLineageOptions options)
        {
            options.EnableLineageTracking = true; // Default: Tracking is enabled
            options.ThrowOnNullSources = false;   // Default: Null sources are ignored
        }
    }
}
