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
	/// A class to retrieve and map Invoice item from Severa to M-Files.
	/// </summary>
	class ItemInvoice : Item, IItem
	{
		/// <summary>		
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemInvoice(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{{ "AccountGUID", "System.String" },
												{ "BusinessUnitGUID", "System.String" },
												{ "CurrencyGUID", "System.String" },
												{ "CurrencyName", "System.String" },
												{ "CurrencyRate", "System.Double" },
												{ "CurrencyShortform", "System.String" },
												{ "Date", "System.DateTime" },
												{ "DueDate", "System.DateTime" },
												{ "EntryDate", "System.DateTime" },
												{ "GUID", "System.String" },
												{ "Number", "System.String" },
												{ "TotalVatAmount", "System.Double" },
												{ "TotalVatExcludedAmount", "System.Double" },
												{ "TotalVatIncludedAmount", "System.Double" },
												{ "InvoiceStatus", "System.String" },
												{ "IsCreditNote", "System.Boolean"},
												{ "IsReimbursed", "System.Boolean"},
												{ "LanguageCode", "System.String" },
												{ "LanguageGUID", "System.String" },
												{ "MainCaseGUID", "System.String" },
												{ "Notes", "System.String" },
												{ "OrderNumber", "System.String" },
												{ "OurReference", "System.String" },
												{ "OverDueInterest", "System.Double" },
												{ "PaymentDate", "System.DateTime" },
												{ "PaymentStatus", "System.String" },
												{ "PaymentTerm", "System.Int32" },
												{ "ReferenceNumber", "System.String" },
												{ "ReimburseInvoiceGUID", "System.String" },
												{ "YourReference", "System.String" } };
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

			List<DataItem> dataItems = new List<DataItem>();
			Invoice[] invoices = m_agent.GetModifiedInvoices( RangeMinUTC, maxRange );
			if( invoices != null )
				foreach( Invoice _invoice in invoices )
				{
					// Break if max result count reached.
					if( recordCount > 0 && recordCount == MaxResults )
						break;

					dataItems.Add( FormDataItem( _invoice ) );
					recordCount++;
				}
			return dataItems;
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Invoice GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneInvoice( GUID ) );
		}



		/// <summary>
		/// Converts an invoice into a data item.
		/// </summary>
		/// <param name="_invoice">Invoice object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Invoice _invoice )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( _invoice, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_invoice">Company object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Invoice _invoice, string ColumnName )
		{
			// Find ColumnName's index in AVAILABLE_COLUMNS table.
			int columnIndex = 0;
			for( ; columnIndex < COLUMNS_COUNT; ++columnIndex )
				if( ColumnName == AVAILABLE_COLUMNS[ columnIndex, 0 ] )
					break;
			if( columnIndex == COLUMNS_COUNT )
				throw new Exception( "Column " + ColumnName + " not found." );

			// Select return value by columnIndex.
			switch( columnIndex )
			{
				case 0:
					return _invoice.AccountGUID;

				case 1:
					return _invoice.BusinessUnitGUID;

				case 2:
					return _invoice.CurrencyGUID;

				case 3:
					return _invoice.CurrencyName;

				case 4:
					return _invoice.CurrencyRate;

				case 5:
					return _invoice.CurrencyShortform;

				case 6:
					return _invoice.Date;

				case 7:
					return _invoice.DueDate;

				case 8:
					return _invoice.EntryDate;

				case 9:
					return _invoice.GUID;

				case 10:
					return _invoice.Number;

				case 11:
					return _invoice.InvoiceDetails.invoiceTotalVatAmount;

				case 12:
					return _invoice.InvoiceDetails.invoiceTotalVatExcludedAmount;

				case 13:
					return _invoice.InvoiceDetails.invoiceTotalVatIncludedAmount;

				case 14:
					return _invoice.InvoiceStatus;

				case 15:
					return _invoice.IsCreditNote;

				case 16:
					return _invoice.IsReimbursed;

				case 17:
					return _invoice.LanguageCode;

				case 18:
					return _invoice.LanguageGUID;

				case 19:
					return _invoice.MainCaseGUID;

				case 20:
					return _invoice.Notes;

				case 21:
					return _invoice.OrderNumber;

				case 22:
					return _invoice.OurReference;

				case 23:
					return _invoice.OverDueInterest;

				case 24:
					return _invoice.PaymentDate;

				case 25:
					return _invoice.PaymentStatus;

				case 26:
					return _invoice.PaymentTerm;

				case 27:
					return _invoice.ReferenceNumber;

				case 28:
					return _invoice.ReimburseInvoiceGUID;

				case 29:
					return _invoice.YourReference;

				default:
					throw new Exception( "Column " + ColumnName + " not found." );
			}
		}
	}
}
