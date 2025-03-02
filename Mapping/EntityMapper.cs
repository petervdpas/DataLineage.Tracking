using System;
using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;
using System.Linq;
using System.Reflection;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// A concrete implementation of <see cref="IEntityMapper"/> that supports data lineage tracking.
    /// </summary>
    public class EntityMapper : IEntityMapper
    {
        private readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapper"/> class.
        /// </summary>
        /// <param name="lineageTracker">The lineage tracker instance for tracking data transformations.</param>
        public EntityMapper(IDataLineageTracker lineageTracker)
        {
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public TResult Map<TSource, TResult>(
            IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, TResult> mappingFunction,
            IDataLineageTracker? lineageTracker = null)
        {
            // Use the passed lineage tracker if available, otherwise use the injected one
            var tracker = lineageTracker ?? _lineageTracker;

            TResult result = mappingFunction.Invoke(sources);

            // Track only explicitly mapped fields
            if (tracker != null && mappingFunction.Target != null)
            {
                foreach (var source in sources)
                {
                    if (source == null) continue;

                    var sourceType = source.GetType();
                    var targetType = typeof(TResult);

                    // Get properties explicitly accessed in the mapping function
                    var mappedFields = GetMappedFields(mappingFunction);
                    if (mappedFields.Count == 0) continue;

                    foreach (var field in mappedFields)
                    {
                        tracker.Track(
                            sourceName: $"{sourceType.Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            sourceEntity: sourceType.Name,
                            sourceField: field,
                            transformationRule: "Explicit Mapping",
                            targetName: $"{targetType.Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            targetEntity: targetType.Name,
                            targetField: field
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
