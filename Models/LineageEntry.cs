using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
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
        public string? SourceSystem { get; set; }

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
        public string? TransformationRule { get; set; }

        /// <summary>
        /// The unique identifier or name of the target instance.
        /// </summary>
        public string? TargetSystem { get; set; }

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
        public bool Validated { get; set; }

        /// <summary>
        /// A list of tags providing additional context about the target field.
        /// </summary>
        public List<string>? Tags { get; set; } = [];

        /// <summary>
        /// The classification of the data using the CIA (Confidentiality, Integrity, Availability) model.
        /// </summary>
        public DataClassification? Classification { get; set; } = new();

        /// <summary>
        /// Flexible metadata object to hold arbitrary key-value data.
        /// </summary>
        public ExpandoObject? MetaExtra { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineageEntry"/> class with default values.
        /// This constructor is required for serialization and dependency injection.
        /// </summary>
        public LineageEntry()
        {
            SourceSystem = string.Empty;
            SourceEntity = string.Empty;
            SourceField = string.Empty;
            TransformationRule = string.Empty;
            TargetSystem = string.Empty;
            TargetEntity = string.Empty;
            TargetField = string.Empty;
            Validated = false;
            Tags = [];
            Classification = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineageEntry"/> class with provided values.
        /// </summary>
        /// <param name="sourceSystem">The unique identifier or name of the source instance.</param>
        /// <param name="sourceEntity">The entity type of the source.</param>
        /// <param name="sourceField">The specific field in the source entity.</param>
        /// <param name="transformationRule">The transformation rule applied.</param>
        /// <param name="targetSystem">The unique identifier or name of the target instance.</param>
        /// <param name="targetEntity">The entity type of the target.</param>
        /// <param name="targetField">The specific field in the target entity.</param>
        /// <param name="validated">Indicates if the transformation has been validated.</param>
        /// <param name="tags">A list of tags providing additional context about the transformation.</param>
        /// <param name="classification">The CIA classification of the data.</param>
        /// <param name="metaExtra">Flexible information field to hold user-defined meta data.</param>
        public LineageEntry(
            string? sourceSystem, string sourceEntity, string sourceField,
            string? transformationRule,
            string? targetSystem, string targetEntity, string targetField,
            bool validated, List<string>? tags, DataClassification? classification, ExpandoObject? metaExtra)
        {
            SourceSystem = sourceSystem;
            SourceEntity = sourceEntity;
            SourceField = sourceField;
            TransformationRule = transformationRule;
            TargetSystem = targetSystem;
            TargetEntity = targetEntity;
            TargetField = targetField;
            Validated = validated;
            Tags = tags;
            Classification = classification;
            MetaExtra = metaExtra;
        }

        /// <summary>
        /// Tracks a data transformation asynchronously using expressions that represent the source and target fields.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="sourceExpr">An expression representing the field in the source entity.</param>
        /// <param name="targetExpr">An expression representing the field in the target entity.</param>
        /// <param name="sourceSystem">The unique identifier or instance name of the source system. (Optional)</param>
        /// <param name="transformationRule">A description of the transformation logic applied to the data. (Optional)</param>
        /// <param name="targetSystem">The unique identifier or instance name of the target system. (Optional)</param>
        /// <param name="validated">Indicates whether the transformation has been validated. (Optional)</param>
        /// <param name="tags">A list of contextual tags related to the transformation. (Optional)</param>
        /// <param name="classification">
        /// A three-digit integer representing CIA levels (Confidentiality-Integrity-Availability).
        /// Example: 123 (C1-I2-A3), 321 (C3-I2-A1), 333 (C3-I3-A3).
        /// If omitted or null, the classification is considered undefined.
        /// </param>
        /// <param name="metaExtra">
        /// A dynamic metadata object containing user-defined properties relevant to the lineage entry.
        /// This may include model references, links, documentation identifiers, or other auxiliary information.
        /// Optional.
        /// </param>
        /// <returns>A task representing the asynchronous tracking operation.</returns>
        public static LineageEntry FromGeneric<TSource, TResult>(            
            Expression<Func<TSource, object>> sourceExpr, 
            Expression<Func<TResult, object>> targetExpr, 
            string? sourceSystem = null, 
            string? transformationRule = null, 
            string? targetSystem = null, 
            bool validated = false,
            List<string>? tags = null,
            int? classification = null,
            ExpandoObject? metaExtra = null)  where TSource : class where TResult : class
        {
            MemberExpression? sourceExpression = GetMemberExpression(sourceExpr);
            MemberExpression? targetExpression = GetMemberExpression(targetExpr);

            return new LineageEntry(
                sourceSystem: sourceSystem, 
                sourceEntity: typeof(TSource).Name,
                sourceField: sourceExpression?.Member.Name ?? "Unresolved",
                transformationRule: transformationRule ?? string.Empty,
                targetSystem: targetSystem, 
                targetEntity: typeof(TResult).Name,
                targetField: targetExpression?.Member.Name ?? "Unresolved",
                validated: validated, 
                tags: tags, 
                classification: classification,
                metaExtra: metaExtra);
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
            return HashCode.Combine(SourceSystem, SourceEntity, SourceField, TransformationRule, TargetSystem, TargetEntity, TargetField);
        }

        /// <summary>
        /// Returns a formatted string showing the transformation path.
        /// </summary>
        /// <returns>A string representation of the data lineage entry.</returns>
        public override string ToString()
        {
            string tagsFormatted = Tags is { Count: > 0 } ? $"[{string.Join(", ", Tags)}]" : "None";
            string metaJson = MetaExtra != null ? JsonSerializer.Serialize(MetaExtra, _jsonOptions) : "null";

            return $"{SourceSystem}.{SourceEntity}.{SourceField} " +
                   $"➡ [{TransformationRule}] ➡ " +
                   $"{TargetSystem}.{TargetEntity}.{TargetField} " +
                   $"(✔: {Validated}, Tags: {tagsFormatted}, Classification: {Classification}, MetaExtra: {metaJson})";
        }

        /// <summary>
        /// Converts this lineage entry to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the lineage entry.</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, _jsonOptions);
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
