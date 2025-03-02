using System.Collections.Generic;
using System.Linq;
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Models;
using DataLineage.Tracking.Sinks;

namespace DataLineage.Tracking.Lineage
{
    /// <summary>
    /// Default implementation of <see cref="IDataLineageTracker"/>.
    /// Stores data lineage records that track the transformation of source data into target fields.
    /// </summary>
    public class DataLineageTracker : IDataLineageTracker
    {
        private readonly HashSet<LineageEntry> _lineageEntries = [];
        private readonly List<ILineageSink> _sinks = [];

        /// <summary>
        /// Initializes a new instance of <see cref="DataLineageTracker"/> with optional sinks.
        /// </summary>
        /// <param name="sinks">Optional sinks for storing lineage entries.</param>
        public DataLineageTracker(IEnumerable<ILineageSink>? sinks = null)
        {
            if (sinks != null)
            {
                _sinks.AddRange(sinks);
            }
        }

        /// <inheritdoc/>
        public void Track(
            string sourceSystem, string sourceEntity, string sourceField, bool sourceBusinessKey, string sourceDescription,
            string transformationRule,
            string targetSystem, string targetEntity, string targetField, bool targetBusinessKey, string targetDescription)
        {
            var newEntry = new LineageEntry(
                sourceSystem, sourceEntity, sourceField, sourceBusinessKey, sourceDescription,
                transformationRule,
                targetSystem, targetEntity, targetField, targetBusinessKey, targetDescription
            );

            // ✅ **Only add if it's a truly new lineage entry**
            if (!_lineageEntries.Contains(newEntry))
            {
                _lineageEntries.Add(newEntry);

                // 🔹 Store lineage in configured sinks
                foreach (var sink in _sinks)
                {
                    sink.StoreLineage([newEntry]);
                }
            }
        }

        /// <inheritdoc/>
        public List<LineageEntry> GetLineage() => _lineageEntries.ToList();
    }
}
