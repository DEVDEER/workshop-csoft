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

        #endregion
    }
}