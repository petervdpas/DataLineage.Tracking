using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DataLineage.Tracking.Models;
using DataLineage.Tracking.Interfaces;

namespace DataLineage.Tracking.Sinks
{
    /// <summary>
    /// Stores lineage data in a relational database using Dapper.
    /// Supports multiple database providers via <see cref="IDbConnection"/>.
    /// </summary>
    public class SqlLineageSink : ILineageSink
    {
        private readonly Func<IDbConnection> _connectionFactory;
        private readonly string _tableName;
        private readonly bool _deleteOnStartup;

        /// <summary>
        /// Initializes a new instance of <see cref="SqlLineageSink"/>.
        /// </summary>
        /// <param name="connectionFactory">
        /// A factory function that provides an <see cref="IDbConnection"/> to the database.
        /// Supports multiple database providers.
        /// </param>
        /// <param name="tableName">The name of the table where lineage data will be stored.</param>
        /// <param name="deleteOnStartup">
        /// If <c>true</c>, the lineage table will be cleared on application startup.
        /// </param>
        public SqlLineageSink(Func<IDbConnection> connectionFactory, string tableName = "DataLineage", bool deleteOnStartup = false)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _tableName = tableName;
            _deleteOnStartup = deleteOnStartup;

            EnsureTableExists();

            if (_deleteOnStartup)
            {
                DeleteAllRecords();
            }
        }

        /// <inheritdoc/>
        public async Task InsertLineageAsync(IEnumerable<LineageEntry> entries)
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string insertQuery = $@"
                INSERT INTO {_tableName} 
                (SourceName, SourceEntity, SourceField, SourceValidated, SourceDescription, 
                 TransformationRule, TargetName, TargetEntity, TargetField, TargetValidated, TargetDescription) 
                VALUES 
                (@SourceName, @SourceEntity, @SourceField, @SourceValidated, @SourceDescription, 
                 @TransformationRule, @TargetName, @TargetEntity, @TargetField, @TargetValidated, @TargetDescription);";

            foreach (var entry in entries)
            {
                if (!await ExistsLineageAsync(entry))
                {
                    await connection.ExecuteAsync(insertQuery, entry);
                }
            }
        }

        /// <inheritdoc/>
        public async Task UpdateLineageAsync(IEnumerable<LineageEntry> entries)
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string updateQuery = $@"
                UPDATE {_tableName} 
                SET 
                    SourceValidated = @SourceValidated,
                    SourceDescription = @SourceDescription,
                    TransformationRule = @TransformationRule,
                    TargetValidated = @TargetValidated,
                    TargetDescription = @TargetDescription
                WHERE 
                    SourceName = @SourceName
                    AND SourceEntity = @SourceEntity
                    AND SourceField = @SourceField
                    AND TargetName = @TargetName
                    AND TargetEntity = @TargetEntity
                    AND TargetField = @TargetField;";

            await connection.ExecuteAsync(updateQuery, entries);
        }

        /// <inheritdoc/>
        public async Task DeleteLineageAsync(IEnumerable<LineageEntry> entries)
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string deleteQuery = $@"
                DELETE FROM {_tableName} 
                WHERE 
                    SourceName = @SourceName
                    AND SourceEntity = @SourceEntity
                    AND SourceField = @SourceField
                    AND TargetName = @TargetName
                    AND TargetEntity = @TargetEntity
                    AND TargetField = @TargetField;";

            await connection.ExecuteAsync(deleteQuery, entries);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<LineageEntry>> GetAllLineageAsync()
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string selectQuery = $"SELECT * FROM {_tableName};";
            return await connection.QueryAsync<LineageEntry>(selectQuery);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsLineageAsync(LineageEntry entry)
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string existsQuery = $@"
                SELECT COUNT(*) FROM {_tableName} 
                WHERE SourceName = @SourceName
                AND SourceEntity = @SourceEntity
                AND SourceField = @SourceField
                AND TargetName = @TargetName
                AND TargetEntity = @TargetEntity
                AND TargetField = @TargetField;";

            int count = await connection.ExecuteScalarAsync<int>(existsQuery, entry);
            return count > 0;
        }

        /// <summary>
        /// Ensures the lineage table exists in the database. Creates it if necessary.
        /// </summary>
        private void EnsureTableExists()
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string createTableQuery = $@"
                CREATE TABLE IF NOT EXISTS {_tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SourceName TEXT NOT NULL,
                    SourceEntity TEXT NOT NULL,
                    SourceField TEXT NOT NULL,
                    SourceValidated BOOLEAN NOT NULL,
                    SourceDescription TEXT,
                    TransformationRule TEXT NOT NULL,
                    TargetName TEXT NOT NULL,
                    TargetEntity TEXT NOT NULL,
                    TargetField TEXT NOT NULL,
                    TargetValidated BOOLEAN NOT NULL,
                    TargetDescription TEXT
                );";

            connection.Execute(createTableQuery);
        }

        /// <summary>
        /// Deletes all lineage records from the table.
        /// </summary>
        private void DeleteAllRecords()
        {
            using var connection = _connectionFactory();
            connection.Open(); // Use synchronous opening

            string deleteQuery = $"DELETE FROM {_tableName};";
            connection.Execute(deleteQuery);
        }
    }
}
