using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mappers that track data lineage.
    /// Implements <see cref="IGenericTracker{TResult}"/> to support both mapping and lineage tracking.
    /// </summary>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class GenericEntityMapper<TResult> : IGenericTracker<TResult> where TResult : class
    {
        /// <summary>
        /// The lineage tracker instance used to record data transformations.
        /// This is available to derived classes for tracking lineage.
        /// </summary>
        protected readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEntityMapper{TResult}"/> class.
        /// </summary>
        /// <param name="lineageTracker">The data lineage tracker used for tracking transformations.</param>
        public GenericEntityMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public abstract TResult Map(List<object> sources);

        /// <inheritdoc/>
        /// <remarks>
        /// This method is a no-op by default. Derived classes should override it to implement specific lineage tracking logic.
        /// </remarks>
        public virtual async Task Track(List<object>? sources = null, TResult? result = null)
        {
            await Task.CompletedTask;
        }
    }
}
