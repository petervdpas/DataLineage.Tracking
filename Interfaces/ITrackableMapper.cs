using System.Threading.Tasks;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mappers that support lineage tracking.
    /// </summary>
    /// <typeparam name="TSource">The type of the source data.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result.</typeparam>
    public interface ITrackableMapper<TSource, TResult> : IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Tracks data lineage asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        Task Track();
    }
}
