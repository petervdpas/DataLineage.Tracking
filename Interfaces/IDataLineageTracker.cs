using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for tracking data lineage during transformations.
    /// Enables mapping operations to capture and record the flow of data from source to target.
    /// </summary>
    public interface IDataLineageTracker
    {
        /// <summary>
        /// Tracks a data transformation asynchronously from a source field to a target field.
        /// </summary>
        /// <param name="sourceName">The unique identifier or instance name of the source.</param>
        /// <param name="sourceEntity">The type of entity the source belongs to.</param>
        /// <param name="sourceField">The specific field in the source entity that was transformed.</param>
        /// <param name="sourceValidated">Indicates whether the source field is approved by governance.</param>
        /// <param name="sourceDescription">Additional context or business meaning of the source field.</param>
        /// <param name="transformationRule">A description of the transformation logic applied to the data.</param>
        /// <param name="targetName">The unique identifier or instance name of the target.</param>
        /// <param name="targetEntity">The type of entity the target belongs to.</param>
        /// <param name="targetField">The specific field in the target entity that received the transformed data.</param>
        /// <param name="targetValidated">Indicates whether the target field is approved by governance.</param>
        /// <param name="targetDescription">Additional context or business meaning of the target field.</param>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task TrackAsync(
            string sourceName, 
            string sourceEntity, 
            string sourceField, 
            bool sourceValidated,
            string sourceDescription, 
            string transformationRule, 
            string targetName, 
            string targetEntity, 
            string targetField,
            bool targetValidated,
            string targetDescription);

        /// <summary>
        /// Retrieves all recorded lineage entries asynchronously, showing how data was mapped from sources to targets.
        /// </summary>
        /// <returns>A task resolving to a list of <see cref="LineageEntry"/> objects representing the transformation history.</returns>
        Task<List<LineageEntry>> GetLineageAsync();
    }
}
