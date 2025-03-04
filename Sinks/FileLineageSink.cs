using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataLineage.Tracking.Models;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Stores lineage data in-memory and periodically saves it to a JSON file.
    /// Supports deleting the file on startup for a fresh lineage record.
    /// Thread-safe using a lock to prevent race conditions.
    /// </summary>
    public class FileLineageSink : ILineageSink
    {
        private readonly string _filePath;
        private readonly bool _deleteOnStartup;
        private readonly List<LineageEntry> _lineageEntries = new(); // In-memory store
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        private readonly SemaphoreSlim _lock = new(1, 1); // Asynchronous lock

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLineageSink"/> class.
        /// </summary>
        /// <param name="filePath">The file path where lineage data will be stored.</param>
        /// <param name="deleteOnStartup">
        /// If <c>true</c>, the file is deleted when the application starts, ensuring a fresh lineage record.
        /// </param>
        public FileLineageSink(string filePath, bool deleteOnStartup = false)
        {
            _filePath = filePath;
            _deleteOnStartup = deleteOnStartup;

            // Delete file on startup if the flag is set
            if (_deleteOnStartup && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            else
            {
                LoadFromFileAsync().Wait(); // Load lineage entries into memory
            }
        }

        /// <inheritdoc/>
        public async Task InsertLineageAsync(IEnumerable<LineageEntry> entries)
        {
            await _lock.WaitAsync();
            try
            {
                _lineageEntries.AddRange(entries);
            }
            finally
            {
                _lock.Release();
            }
            await SaveToFileAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateLineageAsync(IEnumerable<LineageEntry> entries)
        {
            await _lock.WaitAsync();
            try
            {
                foreach (var entry in entries)
                {
                    var existing = _lineageEntries.FirstOrDefault(e => e.Equals(entry));
                    if (existing != null)
                    {
                        _lineageEntries.Remove(existing);
                    }
                    _lineageEntries.Add(entry);
                }
            }
            finally
            {
                _lock.Release();
            }
            await SaveToFileAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteLineageAsync(IEnumerable<LineageEntry> entries)
        {
            await _lock.WaitAsync();
            try
            {
                _lineageEntries.RemoveAll(e => entries.Contains(e));
            }
            finally
            {
                _lock.Release();
            }
            await SaveToFileAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LineageEntry>> GetAllLineageAsync()
        {
            await _lock.WaitAsync();
            try
            {
                return _lineageEntries.ToList(); // Return a copy to prevent race conditions
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsLineageAsync(LineageEntry entry)
        {
            await _lock.WaitAsync();
            try
            {
                return _lineageEntries.Contains(entry);
            }
            finally
            {
                _lock.Release();
            }
        }

                /// <summary>
        /// Loads lineage data from the file into memory asynchronously.
        /// </summary>
        private async Task LoadFromFileAsync()
        {
            if (File.Exists(_filePath))
            {
                await _lock.WaitAsync();
                try
                {
                    string json = await File.ReadAllTextAsync(_filePath);
                    var entries = JsonSerializer.Deserialize<List<LineageEntry>>(json);
                    if (entries != null)
                    {
                        _lineageEntries.AddRange(entries);
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
        }

        /// <summary>
        /// Saves the in-memory lineage data to the file asynchronously.
        /// </summary>
        private async Task SaveToFileAsync()
        {
            await _lock.WaitAsync();
            try
            {
                string outputJson = JsonSerializer.Serialize(_lineageEntries, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, outputJson);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
