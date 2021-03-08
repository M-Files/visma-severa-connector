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
	/// A class to retrieve and map Sales process item from Severa to M-Files.
	/// </summary>
    class ItemSalesProcess : Item, IItem
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
		public ItemSalesProcess(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // Case.GUID
                                               { "Name", "System.String" },
                                               { "DefaultProbability", "System.Int32" },
                                               { "IsActive", "System.Boolean" },
                                               { "IsAlsoItemStatus", "System.Boolean" },
                                               { "IsLost", "System.Boolean" },
                                               { "IsWon", "System.Boolean" },                                               
                                               { "IsOffer", "System.Boolean" },
                                               { "IsInProgress", "System.Boolean" }};
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

			// There is no way to filter sales processes by dates. Retrieve all of them.
            SalesProcess[] salesprocesses = m_agent.GetAllSalesProcesses();
            foreach (SalesProcess _salesprocess in salesprocesses)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

                yield return FormDataItem(_salesprocess);
				recordCount++;
            }
        }


		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Sales process GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneSalesProcess(GUID));
        }

		/// <summary>
		/// Converts an sales process into a data item.
		/// </summary>
		/// <param name="_salesprocess">Sales process object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
        private DataItemSimple FormDataItem(SalesProcess _salesprocess)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_salesprocess, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_salesprocess">Sales process object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(SalesProcess _salesprocess, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _salesprocess.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _salesprocess.Name;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _salesprocess.DefaultProbability;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _salesprocess.IsActive;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return _salesprocess.IsAlsoItemStatus;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return _salesprocess.IsLost;
            }

            if (ColumnName == AVAILABLE_COLUMNS[6, 0])
            {
                return _salesprocess.IsWon;
            }

            if (ColumnName == AVAILABLE_COLUMNS[7, 0])
            {
                return _salesprocess.IsOffer;
            }

            if (ColumnName == AVAILABLE_COLUMNS[8, 0])
            {
                return _salesprocess.IsInProgress;
            }
            
            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
