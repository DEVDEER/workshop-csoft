namespace Logic.DataAccess.TableStorage
{
    using System;
    using System.Linq;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines settings for the table storage adapter.
    /// </summary>
    public class TableStorageAdapterSettings
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">The configuration handle.</param>
        public TableStorageAdapterSettings(IConfiguration config)
        {
            ConnectionString = config["Storage:ConnectionString"];
            TableName = config["Storage:TableName"];
        }

        #endregion

        #region properties

        /// <summary>
        /// The connection string for Azure Storage.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// The name of the table in Azure Storage.
        /// </summary>
        public string TableName { get; }

        #endregion
    }
}