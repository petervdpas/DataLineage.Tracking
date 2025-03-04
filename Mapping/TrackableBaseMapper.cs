using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mappers that track lineage.
    /// </summary>
    /// <typeparam name="TSource">The type of the source entity.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class TrackableBaseMapper<TSource, TResult> : ITrackableMapper<TSource, TResult>
    {
        /// <summary>
        /// The lineage tracker instance used for tracking data transformations.
        /// </summary>
        protected readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableBaseMapper{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="lineageTracker">The data lineage tracker responsible for tracking data transformations.</param>
        public TrackableBaseMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public abstract TResult Map(IEnumerable<TSource> sourceData);

        /// <inheritdoc/>
        public virtual Task Track()
        {
            return Task.CompletedTask;
        }
    }
}
