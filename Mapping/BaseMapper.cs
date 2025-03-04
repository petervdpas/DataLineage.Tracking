using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// Represents an abstract base class for mapping entities from multiple source objects to a result type.
    /// Implements <see cref="IEntityMapper{TSource, TResult}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source entity.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result entity.</typeparam>
    public abstract class BaseMapper<TSource, TResult> : IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Maps a collection of source data entities to the result entity type.
        /// </summary>
        /// <param name="sourceData">The collection of source data to be mapped.</param>
        /// <returns>The mapped result entity.</returns>
        public abstract TResult Map(List<TSource> sourceData);
    }
}
