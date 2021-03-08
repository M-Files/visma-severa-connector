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
	/// A class to retrieve and map Business Unit item from Severa to M-Files.
	/// </summary>
    class ItemBusinessUnit : Item, IItem
    {
		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemBusinessUnit(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // BusinessUnit.GUID
                                               { "Name", "System.String" },
                                               { "CostCenterGUID", "System.String" },
                                               { "ParentBusinessUnitGUID", "System.String" },
                                               { "PaymentTerm", "System.DateTime" },
                                               { "Code", "System.String" },
                                               { "VatNumber", "System.String" }};
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

			// If there is a maximum date range specified, extract it.
			bool maxRangeSpecified = ( RangeMaxUTC != null );
			DateTime maxRange = new DateTime();
			if( maxRangeSpecified )
				maxRange = ( DateTime )RangeMaxUTC;

			// Get the business units based on the range minimum.
            BusinessUnit[] businessunits = m_agent.GetModifiedBusinessUnits(RangeMinUTC);
            foreach (BusinessUnit _businessunit in businessunits)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( _businessunit, maxRange ) )
				{
					yield return FormDataItem( _businessunit );
					recordCount++;
				}
            }
        }


		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="businessUnit">BusinessUnit object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( BusinessUnit businessUnit, DateTime MaxRange )
		{
			// TODO: This can be reenabled in future if necessary, but since it is
			// not obligatory to limit the results, to *just* to the given range,
			// this comparison will just merely slow down the plugin.
			return true;

			// A null modification data always results true.
			// Otherwise, compare the UpdatesTS property to max range.
			/*if( businessUnit.UpdatedTS != null )
				return ( DateTime )businessUnit.UpdatedTS <= MaxRange;
			return true; */
		}


		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Business unit GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneBusinessUnit(GUID));
        }

		/// <summary>
		/// Converts an account into a data item.
		/// </summary>
		/// <param name="BusinessUnit">Business unit object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
        private DataItemSimple FormDataItem(BusinessUnit _businessunit)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_businessunit, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="BusinessUnit">BusinessUnit object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(BusinessUnit _businessunit, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _businessunit.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _businessunit.Name;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _businessunit.CostCenterGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _businessunit.ParentBusinessUnitGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return _businessunit.PaymentTerm;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return _businessunit.Code;
            }

            if (ColumnName == AVAILABLE_COLUMNS[6, 0])
            {
                return _businessunit.VatNumber;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
