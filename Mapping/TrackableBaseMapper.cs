using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mappers that track lineage.
    /// Implements <see cref="ITrackableMapper{TSource, TResult}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
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
        public abstract TResult Map(List<TSource> sources);

        /// <inheritdoc/>
        public virtual async Task Track(List<TSource> sources, TResult result)
        {
            await Task.CompletedTask;
        }
    }
}
