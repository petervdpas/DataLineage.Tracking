using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for generic entity mappers that also support data lineage tracking.
    /// </summary>
    /// <typeparam name="TResult">The type of the mapped target object.</typeparam>
    public interface IGenericTracker<TResult> : IGenericMapper<TResult>
    {
        /// <summary>
        /// Tracks data lineage asynchronously for a mapping operation.
        /// </summary>
        /// <param name="sources">
        /// A collection of source objects of various types that contributed to the mapping.
        /// </param>
        /// <param name="result">The resulting mapped object of type <typeparamref name="TResult"/>.</param>
        /// <returns>
        /// A task that completes once the data lineage has been recorded asynchronously.
        /// </returns>
        Task Track(List<object> sources, TResult result);
    }
}
