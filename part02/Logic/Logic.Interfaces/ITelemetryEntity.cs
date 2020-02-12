namespace commasoft.Workshop.Logic.Interfaces
{
    using System;
    using System.Linq;

    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Must be implemented by all telemetry table entities.
    /// </summary>
    public interface ITelemetryEntity 
    {
        #region properties

        /// <summary>
        /// The serial number of the device.
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// The humidity in %.
        /// </summary>
        long Humidity { get; set; }

        /// <summary>
        /// A unique id for the device message.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The temperature in °C.
        /// </summary>
        long Temperature { get; set; }

        /// <summary>
        /// This wind direction in °.
        /// </summary>
        long WindDirection { get; set; }

        /// <summary>
        /// The wind speed in m/s.
        /// </summary>
        long WindSpeed { get; set; }

        #endregion
    }
}