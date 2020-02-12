namespace Logic.DataAccess.TableStorage
{
    using System;
    using System.Collections.Concurrent;
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
        #region constants

        /// <summary>
        /// Holds the cached entities locally and is handled by <see cref="SyncCacheAsync" />.
        /// </summary>
        private static ConcurrentBag<T> _cache;

        #endregion

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
        public async Task<IEnumerable<T>> GetAllAsync(int? maxEntries = null)
        {
            await SyncCacheAsync();
            var result = _cache.OrderByDescending(e => e.Timestamp).AsQueryable();
            if (maxEntries.HasValue)
            {
                result = result.Take(maxEntries.Value);
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<int> GetCountAsync()
        {
            await SyncCacheAsync();
            return _cache.Count;
        }

        #endregion

        #region methods

        /// <summary>
        /// Is called internally to synchronize the local cache with the table storage.
        /// </summary>
        private async Task SyncCacheAsync()
        {
            var lastRetrieved = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(100));
            if (_cache == null || !_cache.Any())
            {
                // first cache fill
                _cache = new ConcurrentBag<T>();
            }
            else
            {
                // retrieve youngest timestamp
                lastRetrieved = _cache.Max(e => e.Timestamp);
            }
            // init SDK objects
            var storageAccount = CloudStorageAccount.Parse(Settings.ConnectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(Settings.TableName);
            TableContinuationToken token = null;
            // define filter-condition for timestamp
            var condition = TableQuery.GenerateFilterConditionForDate(
                nameof(TelemeryTableEntity.Timestamp),
                QueryComparisons.GreaterThan,
                lastRetrieved);
            // build query
            var query = new TableQuery<T>
            {
                FilterString = condition,
                TakeCount = _cache.Any() ? 10 : default(int?)
            };
            // fetch results from table storage
            var result = new List<T>();
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token, null, null)
                    .ConfigureAwait(false);
                result.AddRange(segment.Results);
                token = segment.ContinuationToken;
            }
            while (token != null);
            result.ForEach(r => _cache.Add(r));
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