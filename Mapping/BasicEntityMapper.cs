using System;
using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// A basic implementation of <see cref="IEntityMapper"/> that performs mapping without lineage tracking.
    /// </summary>
    public class BasicEntityMapper : IEntityMapper
    {
        /// <inheritdoc/>
        public TResult Map<TSource, TResult>(
            IEnumerable<TSource> sources, 
            Func<IEnumerable<TSource>, TResult> mappingFunction, 
            IDataLineageTracker? lineageTracker = null)
        {
            return mappingFunction.Invoke(sources);
        }
    }
}