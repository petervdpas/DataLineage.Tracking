using System;
using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;

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

            // Track mappings
            if (tracker != null)
            {
                foreach (var source in sources)
                {
                    if (source != null)
                    {
                        tracker.Track(
                            sourceName: $"{source.GetType().Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            sourceEntity: source.GetType().Name,
                            sourceField: "Unknown Field",
                            transformationRule: "Generic Mapping Rule",
                            targetName: $"{typeof(TResult).Name}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                            targetEntity: typeof(TResult).Name,
                            targetField: "Unknown Field"
                        );
                    }
                }
            }

            return result;
        }
    }
}
