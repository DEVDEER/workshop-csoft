namespace commasoft.Workshop.Logic.Models
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

        /// <summary>
        /// The serial number of the device.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// The humidity in %.
        /// </summary>
        public long Humidity { get; set; }

        /// <summary>
        /// A unique id for the device message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The temperature in °C.
        /// </summary>
        public long Temperature { get; set; }

        /// <summary>
        /// This wind direction in °.
        /// </summary>
        public long WindDirection { get; set; }

        /// <summary>
        /// The wind speed in m/s.
        /// </summary>
        public long WindSpeed { get; set; }

        #endregion
    }
}