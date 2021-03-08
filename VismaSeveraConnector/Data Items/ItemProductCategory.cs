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
	/// A class to retrieve and map Account item from Severa to M-Files.
	/// </summary>
    class ItemProductCategory : Item, IItem
    {
		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemProductCategory(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ] {{ "GUID", "System.String" },           // ProductCategory.GUID         
                                      { "Name", "System.String" }};          // ProductCategory.Name
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
			
			// Get product categories
			ProductCategory[] pcs = m_agent.GetModifiedProductCategories( RangeMinUTC );
            foreach (ProductCategory pc in pcs)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;				

				// No last modified data available.
                yield return FormDataItem(pc);
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Product category GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneProductCategory(GUID));
        }



		/// <summary>
		/// Converts an account into a data item.
		/// </summary>
		/// <param name="pc">Product category object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
        private DataItemSimple FormDataItem(ProductCategory pc)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(i, GetValueFromObject(pc, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="pc">Product category object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(ProductCategory pc, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return pc.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return pc.Name;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
