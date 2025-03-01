using System;
using System.Collections.Generic;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Mapping
{
    /// <summary>
    /// A wrapper around <see cref="IEntityMapper"/> that adds data lineage tracking.
    /// </summary>
    public class EntityMapper : IEntityMapper
    {
        private readonly IEntityMapper _mapper;
        private readonly IDataLineageTracker _lineageTracker;

        /// <summary>
        /// Initializes a new instance of <see cref="EntityMapper"/>.
        /// </summary>
        /// <param name="mapper">The base mapper that performs transformations.</param>
        /// <param name="lineageTracker">The lineage tracker used to log transformations.</param>
        public EntityMapper(IEntityMapper mapper, IDataLineageTracker lineageTracker)
        {
            _mapper = mapper;
            _lineageTracker = lineageTracker;
        }

        /// <inheritdoc/>
        public TResult Map<TSource, TResult>(
            IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, TResult> mappingFunction,
            IDataLineageTracker? lineageTracker = null)
        {
            // Perform the mapping
            TResult result = _mapper.Map(sources, mappingFunction);

            // If a lineage tracker is provided, track lineage
            if (lineageTracker != null)
            {
                foreach (var source in sources)
                {
                    if (source != null)
                    {
                        lineageTracker.Track(
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
