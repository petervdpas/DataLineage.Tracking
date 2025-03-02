using System;
using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using DataLineage.Tracking.Configuration;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// A concrete implementation of <see cref="IEntityMapper"/> that supports configurable data lineage tracking.
    /// </summary>
    public class EntityMapper : IEntityMapper
    {
        private readonly IDataLineageTracker _lineageTracker;
        private readonly IOptionsSnapshot<DataLineageOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapper"/> class.
        /// </summary>
        /// <param name="lineageTracker">The lineage tracker instance.</param>
        /// <param name="options">Configuration options for mapping and lineage tracking.</param>
        public EntityMapper(IDataLineageTracker lineageTracker, IOptionsSnapshot<DataLineageOptions> options)
        {
            _lineageTracker = lineageTracker;
            _options = options;
        }

        /// <inheritdoc/>
        public TResult Map<TSource, TResult>(
            IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, TResult> mappingFunction,
            IDataLineageTracker? lineageTracker = null)
        {
            // Use injected or explicitly provided lineage tracker based on configuration
            var tracker = _options.Value.EnableLineageTracking ? lineageTracker ?? _lineageTracker : null;

            TResult result = mappingFunction.Invoke(sources);

            // If tracking is enabled and mapping function uses captured fields, track lineage
            if (tracker != null && mappingFunction.Target != null)
            {
                foreach (var source in sources)
                {
                    if (source == null)
                    {
                        if (_options.Value.ThrowOnNullSources)
                            throw new ArgumentNullException(nameof(source), "Source cannot be null");
                        continue;
                    }

                    var sourceType = source.GetType();
                    var targetType = typeof(TResult);

                    // Get explicitly mapped fields using reflection
                    var mappedFields = GetMappedFields(mappingFunction);
                    if (mappedFields.Count == 0) continue;

                    foreach (var field in mappedFields)
                    {
                        tracker.Track(
                            sourceName: $"{sourceType.Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            sourceEntity: sourceType.Name,
                            sourceField: field,
                            sourceValidated: false,
                            sourceDescription: "Auto-generated source field",
                            transformationRule: "Explicit Mapping",
                            targetName: $"{targetType.Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            targetEntity: targetType.Name,
                            targetField: field,
                            targetValidated: false,
                            targetDescription: "Auto-generated target field"
                        );
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts explicitly mapped fields by analyzing the mapping function expression.
        /// </summary>
        private List<string> GetMappedFields<TSource, TResult>(Func<IEnumerable<TSource>, TResult> mappingFunction)
        {
            var fields = new List<string>();

            if (mappingFunction.Target != null)
            {
                var fieldsUsed = mappingFunction.Target.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.FieldType == typeof(string)) // Assume mappings involve string transformations
                    .Select(f => f.Name)
                    .ToList();

                fields.AddRange(fieldsUsed);
            }

            return fields;
        }
    }
}