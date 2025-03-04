using System.Collections.Generic;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mapping a collection of source objects to a single target result.
    /// </summary>
    /// <typeparam name="TResult">The type of the mapped result.</typeparam>
    public interface IGenericMapper<TResult>
    {
        /// <summary>
        /// Maps a collection of heterogeneous source data objects to a result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="sourceData">A collection of source objects of various types to be mapped.</param>
        /// <returns>A mapped result of type <typeparamref name="TResult"/> based on the provided sources.</returns>
        TResult Map(List<object> sourceData);
    }
}
