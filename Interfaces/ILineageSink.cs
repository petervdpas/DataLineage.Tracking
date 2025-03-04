using System.Collections.Generic;
using System.Threading.Tasks;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Defines a contract for managing lineage data storage.
    /// Implementations may store lineage entries in a database, file, or other storage systems.
    /// </summary>
    public interface ILineageSink
    {
        /// <summary>
        /// Inserts new lineage entries into the storage system.
        /// </summary>
        /// <param name="entries">A collection of lineage entries to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InsertLineageAsync(IEnumerable<LineageEntry> entries);

        /// <summary>
        /// Updates existing lineage entries in the storage system.
        /// </summary>
        /// <param name="entries">A collection of lineage entries to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateLineageAsync(IEnumerable<LineageEntry> entries);

        /// <summary>
        /// Deletes specified lineage entries from the storage system.
        /// </summary>
        /// <param name="entries">A collection of lineage entries to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteLineageAsync(IEnumerable<LineageEntry> entries);

        /// <summary>
        /// Retrieves all lineage entries stored in the system.
        /// </summary>
        /// <returns>A task containing a collection of stored lineage entries.</returns>
        Task<IEnumerable<LineageEntry>> GetAllLineageAsync();

        /// <summary>
        /// Checks if a given lineage entry already exists in the storage system.
        /// </summary>
        /// <param name="entry">The lineage entry to check.</param>
        /// <returns>A task resolving to <c>true</c> if the entry exists; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsLineageAsync(LineageEntry entry);
    }
}
