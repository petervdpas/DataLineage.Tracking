using System.Collections.Generic;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mapping source objects to target objects.
    /// </summary>
    /// <typeparam name="TResult">The type of the mapped result.</typeparam>
    public interface IGenericMapper<TResult>
    {
        /// <summary>
        /// Maps a collection of source data objects to the result type.
        /// </summary>
        /// <param name="sourceData">The collection of source objects to be mapped.</param>
        /// <returns>The mapped result object.</returns>
        TResult Map(List<object> sourceData);
    }
}
