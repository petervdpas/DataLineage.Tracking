using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mapping source objects to target objects.
    /// Supports tracking data lineage during transformations.
    /// </summary>
    /// <typeparam name="TSource">The type of the source data.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result.</typeparam>
    public interface IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Maps a collection of source data objects to the result type.
        /// </summary>
        /// <param name="sourceData">The collection of source objects to be mapped.</param>
        /// <returns>The mapped result object.</returns>
        TResult Map(IEnumerable<TSource> sourceData);

        /// <summary>
        /// Tracks data lineage asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task Track();
    }
}
