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
using Severa.Entities.API;


namespace VismaSeveraConnector
{
	/// <summary>
	/// A class to retrieve and map Sales status from Severa to M-Files.
	/// </summary>
    class ItemSalesStatus : Item, IItem
    {

		/// <summary>
		/// This item always returns a complete result set.
		/// </summary>
		public override bool AlwaysCompleteResultSet
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemSalesStatus(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // Case.GUID
                                               { "CaseGUID", "System.String" },
                                               { "SalesProcessGUID", "System.String" },       
                                               { "TimeStamp", "System.DateTime" }};
		}




		/// <summary>
		/// Retrieve multiple items from the Web Service.
		/// </summary>       
		/// <param name="MaxResults">Max amount of results returned.</param>
		/// <param name="RangeMaxUTC">Minimum range for "Modified since".</param>
		/// <param name="RangeMinUTC">Maximum range for "Modified since" or NULL.</param>
		/// <returns>A collection of data items.</returns>
		public override IEnumerable<DataItem> GetMultipleItems(
			DateTime RangeMinUTC,
			DateTime? RangeMaxUTC,
			int MaxResults )
		{
			int recordCount = 0;

			// Retrieve all of the sales statuses, can not determine date ranges.
			SalesStatus[] salesstatuses = m_agent.GetAllSalesStatuses();
            foreach (SalesStatus _salesstatus in salesstatuses)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

                yield return FormDataItem(_salesstatus);
				recordCount++;
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Sales status GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneSalesStatus(GUID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Contact"></param>
        /// <returns></returns>
        private DataItemSimple FormDataItem(SalesStatus _salesstatus)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_salesstatus, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Converts an account into a data item.
		/// </summary>
		/// <param name="_salesstatus">Sales status object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with sales status data.</returns>
        private Object GetValueFromObject(SalesStatus _salesstatus, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _salesstatus.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _salesstatus.CaseGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _salesstatus.SalesProcessGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _salesstatus.TimeStamp;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
