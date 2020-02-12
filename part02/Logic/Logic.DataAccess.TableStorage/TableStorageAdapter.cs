namespace Logic.DataAccess.TableStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Default implementation of a simple table storage adapter.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ITableEntity" /> which is stored in the table.</typeparam>
    public class TableStorageAdapter<T> : ITableStorageAdapter<T>
        where T : ITableEntity, new()
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="settings">The settings for connecting to the storage.</param>
        public TableStorageAdapter(TableStorageAdapterSettings settings)
        {
            Settings = settings;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(Settings.ConnectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(Settings.TableName);
            if (!await table.ExistsAsync())
            {
                return null;
            }
            TableContinuationToken token = null;
            var query = new TableQuery<T>();
            var result = new List<T>();
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token, null, null).ConfigureAwait(false);
                result.AddRange(segment.Results);
                token = segment.ContinuationToken;
            }
            while (token != null);
            return result;
        }

        #endregion

        #region properties

        /// <summary>
        /// The settings passed in using DI.
        /// </summary>
        private TableStorageAdapterSettings Settings { get; }

        #endregion
    }
}