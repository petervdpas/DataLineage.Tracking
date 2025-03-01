using System.Collections.Generic;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for tracking data lineage during transformations.
    /// This allows mapping operations to capture and record the flow of data from source to target.
    /// </summary>
    public interface IDataLineageTracker
    {
        /// <summary>
        /// Tracks a data transformation from a source field to a target field.
        /// </summary>
        /// <param name="sourceName">The unique identifier or instance name of the source.</param>
        /// <param name="sourceEntity">The type of entity the source belongs to.</param>
        /// <param name="sourceField">The specific field in the source entity that was transformed.</param>
        /// <param name="transformationRule">A description of how the data was transformed.</param>
        /// <param name="targetName">The unique identifier or instance name of the target.</param>
        /// <param name="targetEntity">The type of entity the target belongs to.</param>
        /// <param name="targetField">The specific field in the target entity that received the transformed data.</param>
        void Track(
            string sourceName, 
            string sourceEntity, 
            string sourceField, 
            string transformationRule, 
            string targetName, 
            string targetEntity, 
            string targetField);

        /// <summary>
        /// Retrieves the recorded lineage entries, showing how data was mapped from sources to targets.
        /// </summary>
        /// <returns>A list of <see cref="LineageEntry"/> objects representing the transformation history.</returns>
        List<LineageEntry> GetLineage();
    }
}
