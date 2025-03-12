namespace DataLineage.Enums
{
    /// <summary>
    /// Defines the confidentiality levels for data classification.
    /// Confidentiality ensures that data is accessed only by authorized individuals.
    /// </summary>
    public enum ConfidentialityLevel 
    { 
        /// <summary>
        /// Low confidentiality: Public data with no access restrictions.
        /// </summary>
        Low = 1, 

        /// <summary>
        /// Medium confidentiality: Restricted to internal users or departments.
        /// </summary>
        Medium = 2, 

        /// <summary>
        /// High confidentiality: Highly sensitive data with strict access controls.
        /// </summary>
        High = 3 
    }
}