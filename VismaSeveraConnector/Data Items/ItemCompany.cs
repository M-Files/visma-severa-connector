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
	/// A class to retrieve and map Company item from Severa to M-Files.
	/// </summary>
	class ItemCompany : Item, IItem
	{
		/// <summary>		
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemCompany(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{{ "GUID", "System.String" },
											   { "Email", "System.String" },
											   { "Employees", "System.String" },
											   { "Name", "System.String" },
											   { "VatNumber", "System.String" },
											   { "AnnualRevenue", "System.String" },
											   { "Website", "System.String" },
											   { "IndustryGUID", "System.String" },
											   { "HeadOfficeAddressGUID", "System.String" },
											   { "TimezoneGUID", "System.String" } };
		}

		/// <summary>
		/// Retrieve multiple items from the Web Service.
		/// </summary>      
		/// <param name="MaxResults">Max amount of results returned.</param>
		/// <param name="RangeMaxUTC">Minimum range for "Modified since".</param>
		/// <param name="RangeMinUTC">Maximum range for "Modified since" or NULL.</param>
		/// <returns>A collection of COMPANY data items.</returns>
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


			Company[] companies = m_agent.GetModifiedCompanies( RangeMinUTC );
			foreach( Company _company in companies )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( _company, maxRange ) )
				{
					yield return FormDataItem( _company );
					recordCount++;
				}
			}
		}

		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="company">Company object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Company company, DateTime MaxRange )
		{
			// No information available for modification date in company object.
			return true;
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Company GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneCompany( GUID ) );
		}



		/// <summary>
		/// Converts an company into a data item.
		/// </summary>
		/// <param name="_company">Company object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Company _company )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( _company, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_company">Company object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Company _company, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return _company.GUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return _company.Email;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return _company.Employees;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return _company.Name;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return _company.VatNumber;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return _company.AnnualRevenue;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return _company.Website;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return _company.IndustryGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return _company.HeadOfficeAddressGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
				return _company.TimezoneGUID;

			throw new Exception( "Column " + ColumnName + " not found." );

		}
	}
}
