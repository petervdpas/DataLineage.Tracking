namespace DataLineage.Tracking.Configuration
{
    /// <summary>
    /// Configuration options for Data Lineage Tracking and Entity Mapping.
    /// </summary>
    public class DataLineageOptions
    {
        /// <summary>
        /// Enables or disables lineage tracking in the entity mapper.
        /// Default is true.
        /// </summary>
        public bool EnableLineageTracking { get; set; } = true;

        /// <summary>
        /// Enables or disables exception throwing on null source objects.
        /// Default is false (skips nulls).
        /// </summary>
        public bool ThrowOnNullSources { get; set; } = false;

        /// <summary>
        /// Gets or sets the name of the source system in the data lineage tracking process.
        /// This is used to identify the originating system of the data.
        /// </summary>
        public required string? SourceSystemName { get; set; }

        /// <summary>
        /// Gets or sets the name of the target system in the data lineage tracking process.
        /// This is used to identify the destination system where the data is mapped or stored.
        /// </summary>
        public required string? TargetSystemName { get; set; }
    }
}
