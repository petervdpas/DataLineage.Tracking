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
    }
}
