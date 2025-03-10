using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataLineage.Tracking.Configuration;
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
        private readonly DataLineageOptions _options;
        private readonly HashSet<LineageEntry> _lineageEntries = [];
        private readonly List<ILineageSink> _sinks = [];

        /// <summary>
        /// Initializes a new instance of <see cref="DataLineageTracker"/> with optional sinks.
        /// </summary>
        /// <param name="options">Configuration options</param>
        /// <param name="sinks">Optional sinks for storing lineage entries.</param>
        public DataLineageTracker(DataLineageOptions options,  IEnumerable<ILineageSink>? sinks = null)
        {
            _options = options;
            
            if (sinks != null)
            {
                _sinks.AddRange(sinks);
            }
        }

        /// <inheritdoc/>
        public async Task TrackAsync(
            string? sourceSystem, string sourceEntity, string sourceField, bool sourceValidated, string sourceDescription,
            string transformationRule,
            string? targetSystem, string targetEntity, string targetField, bool targetValidated, string targetDescription)
        {
            var newEntry = new LineageEntry(
                sourceSystem ?? _options.SourceSystemName, sourceEntity, sourceField, sourceValidated, sourceDescription,
                transformationRule,
                targetSystem ?? _options.TargetSystemName, targetEntity, targetField, targetValidated, targetDescription
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
        public Task TrackAsync<TSource, TTarget>(
            Expression<Func<TSource, object>> sourceExpr, 
            Expression<Func<TTarget, object>> targetExpr, 
            string? sourceSystem = null, 
            bool sourceValidated = false, 
            string? sourceDescription = null, 
            string? transformationRule = null, 
            string? targetSystem = null, 
            bool targetValidated = false, 
            string? targetDescription = null)
        {
            MemberExpression? sourceExpression = sourceExpr.Body as MemberExpression;
            MemberExpression? targetExpression = targetExpr.Body as MemberExpression;

            return TrackAsync(
                sourceSystem ?? _options.SourceSystemName,
                typeof(TSource).Name,
                sourceExpression?.Member.Name ?? "Unresolved",
                sourceValidated,
                sourceDescription!,
                transformationRule!,
                targetSystem ?? _options.TargetSystemName,
                typeof(TTarget).Name,
                targetExpression?.Member.Name ?? "Unresolved",
                targetValidated,
                targetDescription!
            );
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
