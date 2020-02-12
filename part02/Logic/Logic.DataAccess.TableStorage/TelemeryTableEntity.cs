namespace Logic.DataAccess.TableStorage
{
    using System;
    using System.Linq;

    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Represents one line in the Azure table which stores telemetry data.
    /// </summary>
    public class TelemeryTableEntity : TableEntity
    {
        #region properties

        public string DeviceId { get; set; }

        public long Humidity { get; set; }

        public string Id { get; set; }

        public long Temperature { get; set; }

        public long WindDirection { get; set; }

        public long WindSpeed { get; set; }

        #endregion
    }
}