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

        private static List<T> _cache;

        private async Task SyncCacheAsync()
        {
            DateTimeOffset lastRetrieved = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(100));
            if (_cache == null || !_cache.Any())
            {
                _cache = new List<T>();
            }
            else
            {
                lastRetrieved = _cache.Max(e => e.Timestamp);
            }
            var storageAccount = CloudStorageAccount.Parse(Settings.ConnectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(Settings.TableName);
            TableContinuationToken token = null;
            var condition = TableQuery.GenerateFilterConditionForDate(
                "Timestamp", QueryComparisons.GreaterThan,
                lastRetrieved);
            var query = new TableQuery<T>
            {
                FilterString = condition,
                TakeCount = _cache.Any() ? 10 : default(int?)
            };
            var result = new List<T>();
            do
            {
                try
                {
                    var segment = await table.ExecuteQuerySegmentedAsync(query, token, null, null).ConfigureAwait(false);
                    result.AddRange(segment.Results);
                    token = segment.ContinuationToken;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            while (token != null);
            _cache.AddRange(result);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync(int? maxEntries)
        {
            await SyncCacheAsync();
            var result = _cache.OrderByDescending(e => e.Timestamp).AsQueryable();
            if (maxEntries.HasValue)
            {
                result = result.Take(maxEntries.Value);
            }
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