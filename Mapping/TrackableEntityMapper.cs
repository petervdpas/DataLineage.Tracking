using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for entity mappers that support lineage tracking.
    /// Implements <see cref="IEntityTracker{TSource, TResult}"/> to provide mapping functionality with lineage recording.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class TrackableEntityMapper<TSource, TResult> : IEntityTracker<TSource, TResult> where TSource : class where TResult : class
    {
        /// <summary>
        /// The lineage tracker instance used to record data transformations.
        /// This is available to derived classes for tracking lineage.
        /// </summary>
        protected readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableEntityMapper{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="lineageTracker">The data lineage tracker responsible for tracking data transformations.</param>
        public TrackableEntityMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public abstract TResult Map(TSource sources);

        /// <inheritdoc/>
        /// <remarks>
        /// This method is a no-op by default. Derived classes should override it to implement specific lineage tracking logic.
        /// </remarks>
        public virtual async Task Track(TSource? sources = null, TResult? result = null)
        {
            await Task.CompletedTask;
        }
    }
}
