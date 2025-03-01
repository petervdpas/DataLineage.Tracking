using System.Collections.Generic;
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
        private readonly List<LineageEntry> _lineageEntries = new();

        /// <inheritdoc/>
        public void Track(
            string sourceName, string sourceEntity, string sourceField,
            string transformationRule,
            string targetName, string targetEntity, string targetField)
        {
            _lineageEntries.Add(new LineageEntry(
                sourceName, sourceEntity, sourceField,
                transformationRule, targetName, targetEntity, targetField
            ));
        }

        /// <inheritdoc/>
        public List<LineageEntry> GetLineage()
        {
            return [.. _lineageEntries];
        }
    }
}
