using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            if (sinks != null) _sinks.AddRange(sinks);
        }

        /// <inheritdoc/>
        public async Task TrackAsync(
            string? sourceSystem, string sourceEntity, string sourceField,
            string? transformationRule,
            string? targetSystem, string targetEntity, string targetField, 
            bool validated, List<string>? tags, string? modelReferenceUrl, int? classification = null)
        {

            if (string.IsNullOrWhiteSpace(sourceEntity) || string.IsNullOrWhiteSpace(sourceField) ||
                string.IsNullOrWhiteSpace(targetEntity) || string.IsNullOrWhiteSpace(targetField))
            {
                throw new ArgumentException("Source and Target entity names and fields cannot be null or empty.");
            }

            // Convert int classification to DataClassification
            var convertedClassification = classification.HasValue ? DataClassification.ParseFromInt(classification.Value) : null;

            var newEntry = new LineageEntry(
                sourceSystem: sourceSystem ?? _options.SourceSystemName, 
                sourceEntity: sourceEntity, 
                sourceField: sourceField, 
                transformationRule: transformationRule ?? string.Empty,
                targetSystem: targetSystem ?? _options.TargetSystemName, 
                targetEntity: targetEntity, 
                targetField: targetField,
                validated: validated, 
                tags: tags, 
                modelReferenceUrl: modelReferenceUrl ?? string.Empty, 
                classification: convertedClassification);

            if (await ExistsInSinksAsync(newEntry))
            {
                Console.WriteLine($"[INFO] Lineage entry already exists: {newEntry}");
                return;
            }

            _lineageEntries.Add(newEntry);
            await Task.WhenAll(_sinks.Select(sink => sink.InsertLineageAsync([newEntry])));
        }

        /// <inheritdoc/>
        public Task TrackAsync<TSource, TTarget>(
            Expression<Func<TSource, object>> sourceExpr, 
            Expression<Func<TTarget, object>> targetExpr, 
            string? sourceSystem = null, 
            string? transformationRule = null, 
            string? targetSystem = null, 
            bool validated = false,
            List<string>? tags = null,
            string? modelReferenceUrl = null,
            int? classification = null)
        {
            MemberExpression? sourceExpression = GetMemberExpression(sourceExpr);
            MemberExpression? targetExpression = GetMemberExpression(targetExpr);

            return TrackAsync(
                sourceSystem: sourceSystem ?? _options.SourceSystemName, 
                sourceEntity: typeof(TSource).Name,
                sourceField: sourceExpression?.Member.Name ?? "Unresolved",
                transformationRule: transformationRule ?? string.Empty,
                targetSystem: targetSystem ?? _options.TargetSystemName, 
                targetEntity: typeof(TTarget).Name,
                targetField: targetExpression?.Member.Name ?? "Unresolved",
                validated: validated, 
                tags: tags, 
                modelReferenceUrl: modelReferenceUrl ?? string.Empty, 
                classification: classification);
        }

        /// <inheritdoc/>
        public async Task<List<LineageEntry>> GetLineageAsync()
        {
            var lineageFromSinks = await Task.WhenAll(_sinks.Select(sink => sink.GetAllLineageAsync()));
            var allEntries = lineageFromSinks.SelectMany(entries => entries).Concat(_lineageEntries);
            return [.. allEntries.Distinct()];
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

        /// <summary>
        /// Extracts a MemberExpression from an Expression, handling UnaryExpression cases.
        /// </summary>
        private static MemberExpression? GetMemberExpression<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression;
            }
            else if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand)
            {
                return operand; // Unwrap the cast
            }

            return null; // Unresolved expression
        }
    }
}
