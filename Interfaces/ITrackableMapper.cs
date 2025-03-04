using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mappers that support lineage tracking.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
    /// <typeparam name="TResult">The type of the mapped target object.</typeparam>
    public interface ITrackableMapper<TSource, TResult> : IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Tracks data lineage asynchronously for a mapping operation.
        /// </summary>
        /// <param name="sources">The collection of source objects.</param>
        /// <param name="result">The mapped target object.</param>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task Track(List<TSource> sources, TResult result);
    }
}
