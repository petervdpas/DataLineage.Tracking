namespace DataLineage.Enums
{
    /// <summary>
    /// Defines the availability levels for data classification.
    /// Availability ensures that data remains accessible and operational when needed.
    /// </summary>
    public enum AvailabilityLevel  
    { 
        /// <summary>
        /// Low availability: System downtime is acceptable, and recovery is not critical.
        /// </summary>
        Low = 1, 

        /// <summary>
        /// Medium availability: Temporary loss is tolerable, but rapid recovery is required.
        /// </summary>
        Medium = 2, 

        /// <summary>
        /// High availability: Data must always be accessible with minimal downtime.
        /// </summary>
        High = 3 
    }
}