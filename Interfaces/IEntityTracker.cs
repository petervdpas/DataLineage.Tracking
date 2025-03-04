using System.Threading.Tasks;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for entity mappers that also support data lineage tracking.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TResult">The type of the mapped target object.</typeparam>
    public interface IEntityTracker<TSource, TResult> : IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Tracks data lineage asynchronously for a mapping operation.
        /// </summary>
        /// <param name="sources">The source object involved in the mapping.</param>
        /// <param name="result">The resulting mapped object of type <typeparamref name="TResult"/>.</param>
        /// <returns>
        /// A task that completes once the data lineage has been recorded.
        /// </returns>
        Task Track(TSource sources, TResult result);
    }
}
