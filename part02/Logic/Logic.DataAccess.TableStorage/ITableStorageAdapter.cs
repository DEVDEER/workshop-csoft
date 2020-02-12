namespace Logic.DataAccess.TableStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Must be implemented by all table storage adapters.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ITableEntity" /> which is stored in the table.</typeparam>
    public interface ITableStorageAdapter<T>
        where T : ITableEntity, new()
    {
        #region methods

        /// <summary>
        /// Retrieves entries from the table ordered descently by timestamp.
        /// </summary>
        /// <param name="maxEntries">The optional amount of entries to retrieve or <c>null</c> if all entries should be delivered.</param>
        /// <returns>The list of entries.</returns>
        Task<IEnumerable<T>> GetAllAsync(int? maxEntries);

        #endregion
    }
}