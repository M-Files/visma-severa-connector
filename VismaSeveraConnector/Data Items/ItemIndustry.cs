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
    class ItemIndustry : Item, IItem
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

		public ItemIndustry(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // Industry.GUID
                                               { "Code", "System.String" },
                                               { "Name", "System.String" },       
                                               { "IsActive", "System.Boolean" }};
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

            Industry[] industries = m_agent.GetAllIndustries();
            foreach (Industry _industry in industries)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( _industry, maxRange ) )
				{
					yield return FormDataItem( _industry );
					recordCount++;
				}
            }
        }

		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="_industry">Industry object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Industry _industry, DateTime MaxRange )
		{
			// No information about last modification available.
			return true;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneIndustry(GUID));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Contact"></param>
        /// <returns></returns>
        private DataItemSimple FormDataItem(Industry _industry)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_industry, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Contact"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        private Object GetValueFromObject(Industry _industry, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _industry.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _industry.Code;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _industry.Name;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _industry.IsActive;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
