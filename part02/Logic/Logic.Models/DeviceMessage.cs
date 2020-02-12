namespace commasoft.Workshop.Logic.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the model for a single Device-2-Cloud-message.
    /// </summary>
    public class DeviceMessage
    {
        #region properties

        /// <summary>
        /// The serial number of the device.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// The humidity in %.
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// A unique id for the device message.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The temperature in °C.
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// This wind direction in °.
        /// </summary>
        public int WindDirection { get; set; }

        /// <summary>
        /// The wind speed in m/s.
        /// </summary>
        public int WindSpeed { get; set; }

        #endregion
    }
}