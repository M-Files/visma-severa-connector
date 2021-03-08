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
    class ItemTag : Item, IItem
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
		public ItemTag(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // Case.GUID
                                               { "Context", "System.String" },
                                               { "ExtensionData", "System.String" },
                                               { "IsActive", "System.Boolean" },
                                               { "Keyword", "System.String" },
                                               { "Weight", "System.String" }};
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

			// Get all tags. Cannot restrict the time range.
            Tag[] tags = m_agent.GetAllTags();
            foreach (Tag _tag in tags)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

                yield return FormDataItem(_tag);
				recordCount++;
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Tag GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneTag(GUID));
        }


		/// <summary>
		/// Converts an tag into a data item.
		/// </summary>
		/// <param name="_tag">Account object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with tag data.</returns>
        private DataItemSimple FormDataItem(Tag _tag)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_tag, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_tag">Tag object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(Tag _tag, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _tag.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _tag.Context;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
				// For "legacy" reasons I think we should keep it here.
				// However, it has never worked. There's no way it could
				// have, because ExtensionDataObject is not serializable.
				// Anyone who ever tried to retrieve this column has 
				// certainly received an error.
				// By legacy reasons I mean that it is still showing up 
				// in older column mapping sets and removing it could 
				// break some of these configurations.
				return "";

				// return _tag.ExtensionData;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _tag.IsActive;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return _tag.Keyword;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return _tag.Weight;
            }
            
            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
