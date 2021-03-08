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
	/// A class to retrieve and map Address item from Severa to M-Files.
	/// </summary>
	class ItemAddress : Item, IItem
	{
		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemAddress(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{{ "GUID", "System.String" },
											   { "Addressline", "System.String" },
											   { "Addressline2", "System.String" },
											   { "Addressline3", "System.String" },
											   { "City", "System.String" },
											   { "CountryCode", "System.String" },
											   { "CountryName", "System.String" },
											   { "CountryRegionName", "System.String" },
											   { "Fax", "System.String" },
											   { "Phone", "System.String" },
											   { "PostalCode", "System.String" },
											   { "IsBillingAddress", "System.Boolean" },
											   { "IsPostalAddress", "System.Boolean" },
											   { "IsVisitAddress", "System.Boolean" },
											   { "CompanyGUID", "System.String" },
											   { "CountryGUID", "System.String" },
											   { "CountryRegionGUID", "System.String" }};
		}


		/// <summary>
		/// Retrieve multiple items from the Web Service.
		/// <param name="MaxResults">Max amount of results returned.</param>
		/// <param name="RangeMaxUTC">Minimum range for "Modified since".</param>
		/// <param name="RangeMinUTC">Maximum range for "Modified since" or NULL.</param>
		/// </summary>       
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
				maxRange = (DateTime) RangeMaxUTC;

			//Get the addresses based on the range minimum.
			Address[] addresses = m_agent.GetModifiedAddresses( RangeMinUTC );
			foreach( Address address in addresses )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( address, maxRange ) )
				{
					yield return FormDataItem( address );
					recordCount++;
				}
			}
		}

		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="account">Account object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Address address, DateTime MaxRange )
		{
			// TODO: This can be reenabled in future if necessary, but since it is
			// not obligatory to limit the results, to *just* to the given range,
			// this comparison will just merely slow down the plugin.
			return true;

			// For this object, the last modified date is unavailable.
			/* return true; */
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Address GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneAddress( GUID ) );
		}

		/// <summary>
		/// Converts an address into a data item.
		/// </summary>
		/// <param name="Address">Address object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Address _address )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( _address, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}


		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Account">Address object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Address _address, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return _address.GUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return _address.Addressline;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return _address.Addressline2;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return _address.Addressline3;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return _address.City;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return _address.CountryCode;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return _address.CountryName;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return _address.CountryRegionName;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return _address.Fax;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
			{
				return _address.Phone;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 10, 0 ] )
			{
				return _address.PostalCode;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 11, 0 ] )
			{
				return _address.IsBillingAddress;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 12, 0 ] )
			{
				return _address.IsPostalAddress;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 13, 0 ] )
			{
				return _address.IsVisitAddress;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 14, 0 ] )
			{
				return _address.CompanyGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 15, 0 ] )
			{
				return _address.CountryGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 16, 0 ] )
			{
				return _address.CountryRegionGUID;
			}

			throw new Exception( "Column " + ColumnName + " not found." );

		}

	}
}
