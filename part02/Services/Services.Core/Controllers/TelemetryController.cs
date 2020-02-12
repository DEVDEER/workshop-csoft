namespace commasoft.Workshop.Services.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Logic.Interfaces;
    using Logic.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides endpoints for retrieval of telemetry data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : ControllerBase
    {
        #region member vars

        private readonly ITableStorageAdapter<TelemeryTableEntity> _adapter;

        private readonly ILogger<TelemetryController> _logger;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="adapter">The table adapter to retrieve data.</param>
        public TelemetryController(
            ILogger<TelemetryController> logger,
            ITableStorageAdapter<TelemeryTableEntity> adapter)
        {
            _logger = logger;
            _adapter = adapter;
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves a list of telemtry entries ordered by timestamp descendently.
        /// </summary>
        /// <param name="maxEntries">The optional amount of entries to retrieve or <c>null</c> if all entries should be delivered.</param>
        /// <returns>The list of telemetry entries.</returns>
        [HttpGet]
        public async Task<IEnumerable<TelemeryTableEntity>> Get(int? maxEntries)
        {
            try
            {
                return await _adapter.GetAllAsync(maxEntries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the amount of entries.
        /// </summary>
        /// <returns>The amount of telemetry entries.</returns>
        [HttpGet]
        [Route("Count")]
        public async Task<int> GetCountAsync()
        {
            try
            {
                return await _adapter.GetCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the amount of entries.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <param name="from">The timestamp of the oldest humiditiy.</param>
        /// <param name="to">The timestamp of the younges humiditiy.</param>
        /// <returns>TThe humidity values over time.</returns>
        [HttpGet]
        [Route("{deviceId}/Humidities/{from}/{to}")]
        public async Task<IEnumerable<ISensorTimeModel>> GetHumiditiesAsync(
            string deviceId,
            DateTimeOffset from,
            DateTimeOffset to)
        {
            try
            {
                return await _adapter.GetHumiditiesAsync(deviceId, from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the humidities for a given <paramref name="deviceId" /> for the current day.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <returns>The humidity values over time.</returns>
        [HttpGet]
        [Route("{deviceId}/Humidities/Today")]
        public async Task<IEnumerable<ISensorTimeModel>> GetHumiditiesForCurrentDayAsync(
            string deviceId)
        {
            var to = DateTimeOffset.Now;
            var from = new DateTimeOffset(
                to.Year,
                to.Month,
                to.Day,
                0,
                0,
                0,
                0,
                DateTimeOffset.Now.Offset);
            try
            {
                return await _adapter.GetHumiditiesAsync(deviceId, from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the temperatures for a given <paramref name="deviceId" />.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <param name="from">The timestamp of the oldest temperature.</param>
        /// <param name="to">The timestamp of the younges temperature.</param>
        /// <returns>The temperature values over time.</returns>
        [HttpGet]
        [Route("{deviceId}/Temperatures/{from}/{to}")]
        public async Task<IEnumerable<ISensorTimeModel>> GetTemperaturesAsync(
            string deviceId,
            DateTimeOffset from,
            DateTimeOffset to)
        {
            try
            {
                return await _adapter.GetTemperaturesAsync(deviceId, from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the temperatures for a given <paramref name="deviceId" /> for the current day.
        /// </summary>
        /// <param name="deviceId">The serial number of the device.</param>
        /// <returns>The temperature values over time.</returns>
        [HttpGet]
        [Route("{deviceId}/Temperatures/Today")]
        public async Task<IEnumerable<ISensorTimeModel>> GetTemperaturesForCurrentDayAsync(
            string deviceId)
        {
            var to = DateTimeOffset.Now;
            var from = new DateTimeOffset(
                to.Year,
                to.Month,
                to.Day,
                0,
                0,
                0,
                0,
                DateTimeOffset.Now.Offset);
            try
            {
                return await _adapter.GetTemperaturesAsync(deviceId, from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        #endregion
    }
}