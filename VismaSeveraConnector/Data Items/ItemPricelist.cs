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
	class ItemPricelist : Item, IItem
	{
		/// <summary>		
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemPricelist(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,] { { "CurrencyCode", "System.String" },
												{ "CurrencyGUID", "System.String" },
												{ "GUID", "System.String" },
												{ "IsActive", "System.Boolean" },
												{ "IsDefault", "System.Boolean" },
												{ "IsInternal", "System.Boolean" },
												{ "IsVolumePricing", "System.Boolean" },
												{ "Name", "System.String" } };
		}

		/// <summary>
		/// Retrieve multiple items from the Web Service.
		/// </summary>      
		/// <param name="MaxResults">Max amount of results returned.</param>
		/// <param name="RangeMaxUTC">Minimum range for "Modified since".</param>
		/// <param name="RangeMinUTC">Maximum range for "Modified since" or NULL.</param>
		/// <returns>A collection of PriceList data items.</returns>
		public override IEnumerable<DataItem> GetMultipleItems(
			DateTime RangeMinUTC,
			DateTime? RangeMaxUTC,
			int MaxResults )
		{
			int recordCount = 0;

			Pricelist[] pricelists = m_agent.GetAllPricelists();
			foreach( Pricelist _pricelist in pricelists )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				yield return FormDataItem( _pricelist );
				recordCount++;
			}
		}

		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="timezone">Company object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Timezone timezone, DateTime MaxRange )
		{
			// No information available for modification date in timezone object.
			return true;
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Timezone GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOnePricelist( GUID ) );
		}



		/// <summary>
		/// Converts an company into a data item.
		/// </summary>
		/// <param name="_pricelist">Timezone object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Pricelist _pricelist )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( _pricelist, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_pricelist">Timezone object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Pricelist _pricelist, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
				return _pricelist.CurrencyCode;

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
				return _pricelist.CurrencyGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
				return _pricelist.GUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
				return _pricelist.IsActive;

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
				return _pricelist.IsDefault;

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
				return _pricelist.IsInternal;

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
				return _pricelist.IsVolumePricing;

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
				return _pricelist.Name;

			throw new Exception( "Column " + ColumnName + " not found." );

		}
	}
}
