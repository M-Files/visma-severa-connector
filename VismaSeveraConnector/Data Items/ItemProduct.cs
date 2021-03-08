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
    class ItemProduct : Item, IItem
    {

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemProduct(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // Product.GUID
                                               { "Name", "System.String" },
                                               { "Code", "System.String" },
                                               { "Description", "System.String" }, 
                                               { "MeasurementUnit", "System.String" },                                               
                                               { "UnitCost", "System.Double" },
                                               { "UnitPrice", "System.Double" },
                                               { "UseInWorkTimeEntry", "System.Boolean" },
                                               { "VAT", "System.Double"},
                                               { "ProductCategoryGUID", "System.String" },
                                               { "SalesAccountGUID", "System.String" },
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

			// Get the products based on the range minimum.			
			Product[] products = m_agent.GetModifiedProducts( RangeMinUTC );
            foreach (Product product in products)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Can't currently determine upper time range limit for product.
                yield return FormDataItem(product);
				recordCount++;				
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Product GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneProduct(GUID));
        }

		/// <summary>
		/// Converts an product into a data item.
		/// </summary>
		/// <param name="Product">Product object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with product data.</returns>
        private DataItemSimple FormDataItem(Product Product)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(Product, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Product">Product object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(Product Product, string ColumnName)
        {

            if( ColumnName == AVAILABLE_COLUMNS[0,0] ) 
            {
                return Product.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return Product.Name;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return Product.Code;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return Product.Description;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return Product.MeasurementUnit;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return Product.UnitCost;
            }

            if (ColumnName == AVAILABLE_COLUMNS[6, 0])
            {
                return Product.UnitPrice;
            }

            if (ColumnName == AVAILABLE_COLUMNS[7, 0])
            {
                return Product.UseInWorkTimeEntry;
            }

            if (ColumnName == AVAILABLE_COLUMNS[8, 0])
            {
                return Product.VAT;
            }

            if (ColumnName == AVAILABLE_COLUMNS[9, 0])
            {
                return Product.ProductCategoryGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[10, 0])
            {
                return Product.SalesAccountGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[11, 0])
            {
                return Product.IsActive;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
