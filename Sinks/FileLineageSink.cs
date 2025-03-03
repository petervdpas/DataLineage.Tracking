using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Stores lineage data in a JSON file.
    /// Supports append mode (true = keep history, false = overwrite).
    /// </summary>
    public class FileLineageSink : ILineageSink
    {
        private readonly string _filePath;
        private readonly bool _append;
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        /// <summary>
        /// Initializes a new instance of <see cref="FileLineageSink"/>.
        /// </summary>
        /// <param name="filePath">The file path where lineage data will be stored.</param>
        /// <param name="append">If true, appends new entries instead of overwriting the file.</param>
        public FileLineageSink(string filePath, bool append = true)
        {
            _filePath = filePath;
            _append = append;
        }

        /// <summary>
        /// Stores lineage data in the specified file.
        /// </summary>
        /// <param name="entries">The lineage entries to be stored.</param>
        public void StoreLineage(IEnumerable<LineageEntry> entries)
        {
            List<LineageEntry> existingEntries = new();

            // Load existing data only if appending
            if (_append && File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                existingEntries = JsonSerializer.Deserialize<List<LineageEntry>>(json) ?? [];
            }

            // Add new lineage entries
            existingEntries.AddRange(entries);

            // Write to file (overwrite or append based on _append flag)
            File.WriteAllText(_filePath, JsonSerializer.Serialize(existingEntries, _jsonOptions));
        }
    }
}
