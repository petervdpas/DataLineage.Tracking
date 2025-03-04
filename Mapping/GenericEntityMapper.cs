using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mappers that track lineage.
    /// Implements <see cref="IGenericTracker{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class GenericEntityMapper<TResult> : IGenericTracker<TResult>
    {
        /// <summary>
        /// The lineage tracker instance used for tracking data transformations.
        /// </summary>
        protected readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableEntityMapper{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="lineageTracker">The data lineage tracker responsible for tracking data transformations.</param>
        public GenericEntityMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public abstract TResult Map(List<object> sources);

        /// <inheritdoc/>
        public Task Track(List<object> sources, TResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
