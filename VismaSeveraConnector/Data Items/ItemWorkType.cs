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
	/// A class to retrieve and map Work type item from Severa to M-Files.
	/// </summary>
    class ItemWorkType : Item, IItem
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
		public ItemWorkType(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // WorkType.GUID
                                               { "Name", "System.String" }, 
                                               { "Code", "System.String" },
                                               { "SalesAccountGUID", "System.String" },
                                               { "IsProductive", "System.Boolean"},
                                               { "IsDefault", "System.Boolean"},
                                               { "IsActive", "System.Boolean"}};
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

			// We are not able to retrieve these items by date, so get them all.		
            WorkType[] worktypes = m_agent.GetAllWorkTypes();
            foreach (WorkType _worktype in worktypes)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

                yield return FormDataItem(_worktype);
				recordCount++;
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Work type GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneWorkType(GUID));
        }


		/// <summary>
		/// Converts a work type into a data item.
		/// </summary>
		/// <param name="_worktype">Work type object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with work type data.</returns>
        private DataItemSimple FormDataItem(WorkType _worktype)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_worktype, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_worktype">Work type object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(WorkType _worktype, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _worktype.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _worktype.Name;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _worktype.Code;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _worktype.SalesAccountGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return _worktype.IsProductive;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return _worktype.IsDefault;
            }

            if (ColumnName == AVAILABLE_COLUMNS[6, 0])
            {
                return _worktype.IsActive;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
