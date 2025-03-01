namespace DataLineage.Tracking.Models
{
    /// <summary>
    /// Represents an entry in the data lineage tracking system.
    /// This stores details about how data was transformed from a source to a target.
    /// </summary>
    public class LineageEntry
    {
        /// <summary>
        /// The unique identifier or name of the source instance.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// The entity type of the source.
        /// </summary>
        public string SourceEntity { get; set; }

        /// <summary>
        /// The specific field in the source entity that was used in the transformation.
        /// </summary>
        public string SourceField { get; set; }

        /// <summary>
        /// The rule or logic that was applied to transform the source field into the target field.
        /// </summary>
        public string TransformationRule { get; set; }

        /// <summary>
        /// The unique identifier or name of the target instance.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// The entity type of the target.
        /// </summary>
        public string TargetEntity { get; set; }

        /// <summary>
        /// The specific field in the target entity that received the transformed data.
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineageEntry"/> class.
        /// </summary>
        /// <param name="sourceName">The unique name of the source instance.</param>
        /// <param name="sourceEntity">The entity type of the source.</param>
        /// <param name="sourceField">The field in the source entity.</param>
        /// <param name="transformationRule">The transformation rule applied.</param>
        /// <param name="targetName">The unique name of the target instance.</param>
        /// <param name="targetEntity">The entity type of the target.</param>
        /// <param name="targetField">The field in the target entity.</param>
        public LineageEntry(
            string sourceName, string sourceEntity, string sourceField,
            string transformationRule,
            string targetName, string targetEntity, string targetField)
        {
            SourceName = sourceName;
            SourceEntity = sourceEntity;
            SourceField = sourceField;
            TransformationRule = transformationRule;
            TargetName = targetName;
            TargetEntity = targetEntity;
            TargetField = targetField;
        }

        /// <summary>
        /// Returns a string representation of the lineage entry.
        /// </summary>
        /// <returns>A formatted string showing the data transformation path.</returns>
        public override string ToString()
        {
            return $"{SourceName}.{SourceEntity}.{SourceField} → [{TransformationRule}] → {TargetName}.{TargetEntity}.{TargetField}";
        }
    }
}
