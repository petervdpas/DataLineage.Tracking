using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
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
        /// <param name="sourceSystem">The unique identifier or instance name of the source.</param>
        /// <param name="sourceEntity">The type of entity the source belongs to.</param>
        /// <param name="sourceField">The specific field in the source entity that was transformed.</param>
        /// <param name="transformationRule">A description of the transformation logic applied to the data.</param>
        /// <param name="targetSystem">The unique identifier or instance name of the target.</param>
        /// <param name="targetEntity">The type of entity the target belongs to.</param>
        /// <param name="targetField">The specific field in the target entity that received the transformed data.</param>
        /// <param name="validated">Indicates whether the transformation has been validated.</param>
        /// <param name="tags">A list of contextual tags related to the transformation.</param>
        /// <param name="classification">
        /// A three-digit integer representing CIA levels (Confidentiality-Integrity-Availability).
        /// Example: 123 (C1-I2-A3), 321 (C3-I2-A1), 333 (C3-I3-A3).
        /// If omitted or null, the classification is considered undefined.
        /// </param>
        /// <param name="metaExtra">
        /// A dynamic metadata object containing user-defined properties relevant to the lineage entry.
        /// This may include model references, links, documentation identifiers, or other auxiliary information.
        /// Optional.
        /// </param>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task TrackAsync(
            string? sourceSystem,
            string sourceEntity,
            string sourceField,
            string? transformationRule,
            string? targetSystem,
            string targetEntity,
            string targetField,
            bool validated,
            List<string>? tags,
            int? classification = null,
            ExpandoObject? metaExtra = null);

        /// <summary>
        /// Tracks a data transformation asynchronously using expressions that represent the source and target fields.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TTarget">The type of the target entity.</typeparam>
        /// <param name="sourceExpression">An expression representing the field in the source entity.</param>
        /// <param name="targetExpression">An expression representing the field in the target entity.</param>
        /// <param name="sourceSystem">The unique identifier or instance name of the source system. (Optional)</param>
        /// <param name="transformationRule">A description of the transformation logic applied to the data. (Optional)</param>
        /// <param name="targetSystem">The unique identifier or instance name of the target system. (Optional)</param>
        /// <param name="validated">Indicates whether the transformation has been validated. (Optional)</param>
        /// <param name="tags">A list of contextual tags related to the transformation. (Optional)</param>
        /// <param name="classification">
        /// A three-digit integer representing CIA levels (Confidentiality-Integrity-Availability).
        /// Example: 123 (C1-I2-A3), 321 (C3-I2-A1), 333 (C3-I3-A3).
        /// If omitted or null, the classification is considered undefined.
        /// </param>
        /// <param name="metaExtra">
        /// A dynamic metadata object containing user-defined properties relevant to the lineage entry.
        /// This may include model references, links, documentation identifiers, or other auxiliary information.
        /// Optional.
        /// </param>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task TrackAsync<TSource, TTarget>(
            Expression<Func<TSource, object>> sourceExpression,
            Expression<Func<TTarget, object>> targetExpression,
            string? sourceSystem = null,
            string? transformationRule = null,
            string? targetSystem = null,
            bool validated = false,
            List<string>? tags = null,
            int? classification = null,
            ExpandoObject? metaExtra = null);

        /// <summary>
        /// Retrieves all recorded lineage entries asynchronously, showing how data was mapped from sources to targets.
        /// </summary>
        /// <returns>A task resolving to a list of <see cref="LineageEntry"/> objects representing the transformation history.</returns>
        Task<List<LineageEntry>> GetLineageAsync();
    }
}
