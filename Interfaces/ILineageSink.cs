using System.Collections.Generic;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Defines a contract for storing lineage entries.
    /// </summary>
    public interface ILineageSink
    {
        /// <summary>
        /// Stores lineage entries in a custom storage.
        /// </summary>
        /// <param name="entries">The list of lineage entries to store.</param>
        void StoreLineage(IEnumerable<LineageEntry> entries);
    }
}
