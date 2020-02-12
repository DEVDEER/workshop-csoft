namespace commasoft.Workshop.Logic.Models
{
    using System;
    using System.Linq;

    using Interfaces;

    public class SensorTimeModel : ISensorTimeModel
    {
        #region constructors and destructors

        public SensorTimeModel(DateTimeOffset timeStamp, long value)
        {
            Timestamp = timeStamp;
            Value = value;
        }

        #endregion

        #region explicit interfaces

        public DateTimeOffset Timestamp { get; set; }

        public long Value { get; set; }

        #endregion
    }
}