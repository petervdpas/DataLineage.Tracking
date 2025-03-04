using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task TrackAsync(
            string sourceSystem, string sourceEntity, string sourceField, bool sourceBusinessKey, string sourceDescription,
            string transformationRule,
            string targetSystem, string targetEntity, string targetField, bool targetBusinessKey, string targetDescription)
        {
            var newEntry = new LineageEntry(
                sourceSystem, sourceEntity, sourceField, sourceBusinessKey, sourceDescription,
                transformationRule,
                targetSystem, targetEntity, targetField, targetBusinessKey, targetDescription
            );

            // ✅ **Check existence before adding**
            bool alreadyExists = await ExistsInSinksAsync(newEntry);

            if (!alreadyExists)
            {
                _lineageEntries.Add(newEntry);

                // 🔹 Store lineage in configured sinks asynchronously
                var insertTasks = _sinks.Select(sink => sink.InsertLineageAsync(new[] { newEntry }));
                await Task.WhenAll(insertTasks);
            }
        }

        /// <inheritdoc/>
        public async Task<List<LineageEntry>> GetLineageAsync()
        {
            var lineageFromSinks = await Task.WhenAll(_sinks.Select(sink => sink.GetAllLineageAsync()));
            var allEntries = lineageFromSinks.SelectMany(entries => entries).Concat(_lineageEntries);
            return allEntries.Distinct().ToList();
        }

        /// <summary>
        /// Checks if a lineage entry already exists in any of the sinks.
        /// </summary>
        /// <param name="entry">The lineage entry to check.</param>
        /// <returns>True if the entry exists in any sink, otherwise false.</returns>
        private async Task<bool> ExistsInSinksAsync(LineageEntry entry)
        {
            var existenceChecks = _sinks.Select(sink => sink.ExistsLineageAsync(entry));
            var results = await Task.WhenAll(existenceChecks);
            return results.Any(exists => exists);
        }
    }
}
