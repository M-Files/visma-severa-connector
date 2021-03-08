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
	class ItemAccount : Item, IItem
	{

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemAccount(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{ { "GUID", "System.String" },  // Account.GUID
                                               { "Name", "System.String" },  // Account.Name
                                               { "CompanyGUID", "System.String" },  // Account.CompanyGUID
                                               { "AccountOwnerUserGUID", "System.String" },  // Account.AccountOwnerUserGUID
                                               { "eInvoiceAddress", "System.String" },  // Account.eInvoiceAddress
                                               { "eInvoiceOperator", "System.String" },  // Account.eInvoiceOperator
                                               { "IsActive", "System.Boolean" },  // Account.IsActive
                                               { "IsInternal", "System.Boolean" },  // Account.IsInternal
                                               { "LanguageCode", "System.String" },  // Account.LanguageCode
                                               { "PaymentTerm", "System.Int32" },  // Account.PaymentTerm
                                               { "AccountRating", "System.Int32" },  // Account.AccountRating
                                               { "Number", "System.String" },  // Account.Number
                                               { "PricelistGUID", "System.String" },  // Account.PricelistGUID
                                               { "Notes", "System.String" },  // Account.Notes
                                               { "OverdueInterest", "System.Double" },
											   { "ReverseCharge", "System.Boolean" },
											   { "InvoicingVat", "System.Double" },
											   { "LanguageGUID", "System.String" } };
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

			// Get the accounts based on the range minimum.
			Account[] accounts = m_agent.GetModifiedAccounts( RangeMinUTC );

			foreach( Account account in accounts )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( account, maxRange ) )
				{
					yield return FormDataItem( account );
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
		private bool MatchesMaxRange( Account account, DateTime MaxRange )
		{
			// TODO: This can be reenabled in future if necessary, but since it is
			// not obligatory to limit the results, to *just* to the given range,
			// this comparison will just merely slow down the plugin.
			return true;

			// A null modification data always results true.
			// Otherwise, compare the UpdatesTS property to max range.
			/*if( account.UpdatedTS != null )
				return ( DateTime )account.UpdatedTS <= MaxRange;			
			return true;*/
		}


		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Account GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneAccount( GUID ) );
		}



		/// <summary>
		/// Converts an account into a data item.
		/// </summary>
		/// <param name="Account">Account object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Account Account )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( Account, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Account">Account object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Account Account, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return Account.GUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return Account.Name;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return Account.CompanyGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return Account.AccountOwnerUserGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return Account.eInvoiceAddress;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return Account.eInvoiceOperator;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return Account.IsActive;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return Account.IsInternal;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return Account.LanguageCode;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
			{
				return Account.PaymentTerm;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 10, 0 ] )
			{
				return Account.AccountRating;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 11, 0 ] )
			{
				return Account.Number;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 12, 0 ] )
			{
				return Account.PricelistGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 13, 0 ] )
			{
				return Account.Notes;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 14, 0 ] )
				return Account.OverdueInterest;

			if( ColumnName == AVAILABLE_COLUMNS[ 15, 0 ] )
				return Account.ReverseCharge;

			if( ColumnName == AVAILABLE_COLUMNS[ 16, 0 ] )
				return Account.InvoicingVat;

			if( ColumnName == AVAILABLE_COLUMNS[ 17, 0 ] )
				return Account.LanguageGUID;

			throw new Exception( "Column " + ColumnName + " not found." );

		}


	}
}
