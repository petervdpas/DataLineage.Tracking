using System;
using System.Collections.Generic;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mapping source objects to target objects.
    /// Supports tracking data lineage during transformations.
    /// </summary>
    public interface IEntityMapper
    {
        /// <summary>
        /// Maps a collection of source objects to a target object.
        /// Optionally supports data lineage tracking.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TResult">The type of the target object.</typeparam>
        /// <param name="sources">The collection of source objects.</param>
        /// <param name="mappingFunction">The function to transform sources into a target object.</param>
        /// <param name="lineageTracker">An optional lineage tracker to record mapping details.</param>
        /// <returns>The mapped target object.</returns>
        TResult Map<TSource, TResult>(
            IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, TResult> mappingFunction,
            IDataLineageTracker? lineageTracker = null
        );
    }
}
