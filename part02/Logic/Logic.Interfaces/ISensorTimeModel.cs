namespace commasoft.Workshop.Logic.Interfaces
{
    using System;
    using System.Linq;

    /// <summary>
    /// Must be implemented by models which deliver single sensor values over time.
    /// </summary>
    public interface ISensorTimeModel
    {
        #region properties

        DateTimeOffset Timestamp { get; set; }

        long Value { get; set; }

        #endregion
    }
}