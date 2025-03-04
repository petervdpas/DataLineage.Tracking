namespace DataLineage.Tracking.Interfaces
{
    /// <summary>
    /// Defines a generic interface for mapping source objects to target objects.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TResult">The type of the mapped result object.</typeparam>
    public interface IEntityMapper<TSource, TResult>
    {
        /// <summary>
        /// Maps a source object to a target object of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="sourceData">The source object to be mapped.</param>
        /// <returns>The mapped object of type <typeparamref name="TResult"/>.</returns>
        TResult Map(TSource sourceData);
    }
}