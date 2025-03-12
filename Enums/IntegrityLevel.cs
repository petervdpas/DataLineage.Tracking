namespace DataLineage.Enums
{
    /// <summary>
    /// Defines the integrity levels for data classification.
    /// Integrity ensures that data remains accurate, consistent, and unaltered without authorization.
    /// </summary>
    public enum IntegrityLevel   
    { 
        /// <summary>
        /// Low integrity: Minor modifications are acceptable without strict validation.
        /// </summary>
        Low = 1, 

        /// <summary>
        /// Medium integrity: Controlled modifications are allowed with verification.
        /// </summary>
        Medium = 2, 

        /// <summary>
        /// High integrity: No unauthorized modifications are permitted.
        /// </summary>
        High = 3 
    }
}