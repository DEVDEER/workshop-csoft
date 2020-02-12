namespace commasoft.Workshop.Logic.Interfaces
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
        where T : ITableEntity, ITelemetryEntity, new()
    {
        #region methods

        /// <summary>
        /// Retrieves entries from the table ordered descently by timestamp.
        /// </summary>
        /// <param name="maxEntries">The optional amount of entries to retrieve or <c>null</c> if all entries should be delivered.</param>
        /// <returns>The list of entries.</returns>
        Task<IEnumerable<T>> GetAllAsync(int? maxEntries = null);

        /// <summary>
        /// Retrieves the amount of all telemetry items in the storage.
        /// </summary>
        /// <returns>The amount of telemetry entries.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Retrieves the temperatures for a given <paramref name="deviceId" />.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <param name="from">The timestamp of the oldest temperature.</param>
        /// <param name="to">The timestamp of the younges temperature.</param>
        /// <returns>The temperatures ordered by timestamp.</returns>
        Task<IEnumerable<ISensorTimeModel>> GetTemperaturesAsync(
            string deviceId,
            DateTimeOffset @from,
            DateTimeOffset to);

        /// <summary>
        /// Retrieves the humidities for a given <paramref name="deviceId" />.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <param name="from">The timestamp of the oldest humiditiy.</param>
        /// <param name="to">The timestamp of the younges humiditiy.</param>
        /// <returns>The humidities ordered by timestamp.</returns>
        Task<IEnumerable<ISensorTimeModel>> GetHumiditiesAsync(
            string deviceId,
            DateTimeOffset @from,
            DateTimeOffset to);

        #endregion
    }
}