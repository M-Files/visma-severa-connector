/*

This code is provided as a reference sample only and has no explicit or implicit support
as to its nature, completeness, nor function.  Please see the license file
(included in this repository) for more details.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MFiles.Server.Extensions;


namespace VismaSeveraConnector
{
	/// <summary>
	/// IDataSource implementation for M-Files.
	/// </summary>
    public class VSDataSource : IDataSource
    {
        /// <summary>
        /// Open connection to Severa.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="configurationId">Connection GUID.</param>
        /// <returns>VSDataSourceConnection object.</returns>
        public IDataSourceConnection OpenConnection(string connectionString, System.Guid configurationId)
        {           
            return new VSDataSourceConnection(connectionString);
        }

        /// <summary>
        /// Can the data be altered with this plugin? (No, not implemented.)
        /// </summary>
        /// <returns>False.</returns>
        public bool CanAlterData()
        {
            return false;
        }

    }
}
