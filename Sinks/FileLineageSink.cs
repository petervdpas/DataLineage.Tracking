using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Stores lineage data in a JSON file.
    /// Supports append and overwrite modes.
    /// </summary>
    public class FileLineageSink : ILineageSink
    {
        private readonly string _filePath;
        private readonly bool _overwrite;
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        /// <summary>
        /// Initializes a new instance of <see cref="FileLineageSink"/>.
        /// </summary>
        /// <param name="filePath">The file path where lineage data will be stored.</param>
        /// <param name="overwrite">Whether to overwrite the file (true) or append new lineage entries (false).</param>
        public FileLineageSink(string filePath, bool overwrite = false)
        {
            _filePath = filePath;
            _overwrite = overwrite;
        }

        /// <summary>
        /// Stores lineage data in the specified file.
        /// </summary>
        /// <param name="entries">The lineage entries to be stored.</param>
        public void StoreLineage(IEnumerable<LineageEntry> entries)
        {
            List<LineageEntry> existingEntries = new();

            if (!_overwrite && File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                existingEntries = JsonSerializer.Deserialize<List<LineageEntry>>(json) ?? [];
            }

            existingEntries.AddRange(entries);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(existingEntries, _jsonOptions));
        }
    }
}