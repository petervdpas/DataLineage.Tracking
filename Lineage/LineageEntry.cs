namespace DataLineage.Tracking.Lineage
{
    public class LineageEntry
    {
        public string SourceName { get; set; }
        public string SourceEntity { get; set; }
        public string SourceField { get; set; }
        public string TransformationRule { get; set; }
        public string TargetName { get; set; }
        public string TargetEntity { get; set; }
        public string TargetField { get; set; }

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

        public override string ToString()
        {
            return $"{SourceName}.{SourceEntity}.{SourceField} → [{TransformationRule}] → {TargetName}.{TargetEntity}.{TargetField}";
        }
    }
}
