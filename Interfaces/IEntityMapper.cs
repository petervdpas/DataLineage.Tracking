using System.Collections.Generic;

namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines an interface for mapping source objects to target objects.
    /// </summary>
    /// <typeparam name="TSource">The type of the source data.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result.</typeparam>
    public interface IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Map a the source data type to the result type.
        /// </summary>
        /// <param name="sourceData">The source type to be mapped.</param>
        /// <returns>The mapped result object(type).</returns>
        TResult Map(TSource sourceData);
    }
}
