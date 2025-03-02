using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataLineage.Tracking.Models
{
    /// <summary>
    /// Represents an entry in the data lineage tracking system.
    /// This stores details about how data was transformed from a source to a target.
    /// </summary>
    public class LineageEntry
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true, // Pretty-print JSON
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault // Avoid writing default values
        };

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
        /// Indicates if the source field has been validated and approved by governance.
        /// </summary>
        public bool SourceValidated { get; set; }

        /// <summary>
        /// A description providing additional context about the source field.
        /// </summary>
        public string SourceDescription { get; set; }

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
        /// Indicates if the target field has been validated and approved by governance.
        /// </summary>
        public bool TargetValidated { get; set; }

        /// <summary>
        /// A description providing additional context about the target field.
        /// </summary>
        public string TargetDescription { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineageEntry"/> class with default values.
        /// This constructor is required for serialization and dependency injection.
        /// </summary>
        public LineageEntry()
        {
            SourceName = string.Empty;
            SourceEntity = string.Empty;
            SourceField = string.Empty;
            SourceValidated = false;
            SourceDescription = string.Empty;
            TransformationRule = string.Empty;
            TargetName = string.Empty;
            TargetEntity = string.Empty;
            TargetField = string.Empty;
            TargetValidated = false;
            TargetDescription = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineageEntry"/> class with provided values.
        /// </summary>
        /// <param name="sourceName">The unique identifier or name of the source instance.</param>
        /// <param name="sourceEntity">The entity type of the source.</param>
        /// <param name="sourceField">The specific field in the source entity.</param>
        /// <param name="sourceValidated">Indicates if the source field is validated.</param>
        /// <param name="sourceDescription">A description of the source field.</param>
        /// <param name="transformationRule">The transformation rule applied.</param>
        /// <param name="targetName">The unique identifier or name of the target instance.</param>
        /// <param name="targetEntity">The entity type of the target.</param>
        /// <param name="targetField">The specific field in the target entity.</param>
        /// <param name="targetValidated">Indicates if the target field is validated.</param>
        /// <param name="targetDescription">A description of the target field.</param>
        public LineageEntry(
            string sourceName, string sourceEntity, string sourceField, bool sourceValidated, string sourceDescription,
            string transformationRule,
            string targetName, string targetEntity, string targetField, bool targetValidated, string targetDescription)
        {
            SourceName = sourceName;
            SourceEntity = sourceEntity;
            SourceField = sourceField;
            SourceValidated = sourceValidated;
            SourceDescription = sourceDescription;
            TransformationRule = transformationRule;
            TargetName = targetName;
            TargetEntity = targetEntity;
            TargetField = targetField;
            TargetValidated = targetValidated;
            TargetDescription = targetDescription;
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="LineageEntry"/> are equal.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is LineageEntry entry &&
                   SourceEntity == entry.SourceEntity &&
                   SourceField == entry.SourceField &&
                   TransformationRule == entry.TransformationRule &&
                   TargetEntity == entry.TargetEntity &&
                   TargetField == entry.TargetField;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>An integer hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(SourceEntity, SourceField, TransformationRule, TargetEntity, TargetField);
        }

        /// <summary>
        /// Returns a formatted string showing the transformation path.
        /// </summary>
        /// <returns>A string representation of the data lineage entry.</returns>
        public override string ToString()
        {
            return $"{SourceName}.{SourceEntity}.{SourceField} " +
                   $"(✔: {SourceValidated}, {SourceDescription}) " +
                   $"➡ [{TransformationRule}] ➡ " +
                   $"{TargetName}.{TargetEntity}.{TargetField} " +
                   $"(✔: {TargetValidated}, {TargetDescription})";
        }

        /// <summary>
        /// Converts this lineage entry to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the lineage entry.</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, _jsonOptions);
        }
    }
}
