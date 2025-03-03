using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mapping entities from a source type to a result type.
    /// Implements <see cref="IEntityMapper{TSource, TResult}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source entity.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class BaseMapper<TSource, TResult> : IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// The lineage tracker instance used for tracking data transformations.
        /// </summary>
        protected readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMapper{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="lineageTracker">The data lineage tracker responsible for tracking data transformations.</param>
        public BaseMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <summary>
        /// Maps a collection of source data entities to the result entity type.
        /// </summary>
        /// <param name="sourceData">The collection of source data to be mapped.</param>
        /// <returns>The mapped result entity.</returns>
        public abstract TResult Map(IEnumerable<TSource> sourceData);

        /// <summary>
        /// Tracks data lineage asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public abstract Task Track();
    }
}
