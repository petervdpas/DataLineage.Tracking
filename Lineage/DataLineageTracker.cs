using System.Collections.Generic;
using System.Linq;
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Lineage
{
    /// <summary>
    /// Default implementation of <see cref="IDataLineageTracker"/>.
    /// Stores data lineage records that track the transformation of source data into target fields.
    /// </summary>
    public class DataLineageTracker : IDataLineageTracker
    {
        private readonly HashSet<LineageEntry> _lineageEntries = new();

        /// <inheritdoc/>
        public void Track(
            string sourceName, string sourceEntity, string sourceField, bool sourceBusinessKey, string sourceDescription,
            string transformationRule,
            string targetName, string targetEntity, string targetField, bool targetBusinessKey, string targetDescription)
        {
            var newEntry = new LineageEntry(
                sourceName, sourceEntity, sourceField, sourceBusinessKey, sourceDescription,
                transformationRule,
                targetName, targetEntity, targetField, targetBusinessKey, targetDescription
            );

            // ✅ **Only add if it's a truly new lineage entry**
            if (!_lineageEntries.Contains(newEntry))
            {
                _lineageEntries.Add(newEntry);
            }
        }

        /// <inheritdoc/>
        public List<LineageEntry> GetLineage()
        {
            return _lineageEntries.ToList();
        }
    }
}
