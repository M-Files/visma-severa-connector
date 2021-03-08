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

namespace VismaSeveraConnector
{
	/// <summary>
	/// Legacy mode for versions prior to MF9.
	/// </summary>
	public class VSDataSourceConnectionLegacy : IDataSourceConnection
	{

		/// <summary>
		/// Get a minimum range.
		/// This is either the minimum defined by M-Files server or 100 years back from now, if the first is null.
		/// </summary>
		private DateTime RangeMinUTC
		{
			get
			{
				return ( m_rangeMinUTC != null ) ? (DateTime) m_rangeMinUTC : DateTime.Now.AddYears( -100 );
			}

		}

		private int m_maxItemsToRetrieve = -1;
		private DateTime? m_rangeMinUTC;
		private DateTime? m_rangeMaxUTC;

		private VSInterfaceAgent m_agent;
		private IItem m_item;

		/// <summary>
		/// Initialize the connection.
		/// </summary>
		/// <param name="ConnectionString">Connection string</param>
		public VSDataSourceConnectionLegacy( string ConnectionString )
		{
			m_item = null;

			// Extract the properties from connection string.
			string apiKey = GetValue( "apikey", ConnectionString );
			string endpoint = GetValue( "endpoint", ConnectionString );
			string certificateFile = GetValue( "certificate", ConnectionString );
			string debugLog = GetValue( "debuglogfile", ConnectionString );
			if( !string.IsNullOrEmpty( debugLog ) )
				Logger.TurnOnDebugLogging( debugLog );

			// Initialize interface agent.
			m_agent = new VSInterfaceAgent( apiKey, endpoint, certificateFile );

			Logger.DebugLog( "Data source connection created.\r\nConnection string: " + ConnectionString );
		}

		/// <summary>
		/// Can we get one item by its identifier?
		/// </summary>
		/// <returns>True.</returns>
		public bool CanGetOneItem()
		{
			// Always true.
			return true;
		}

		/// <summary>
		/// Called upon the connection closing, void.
		/// </summary>
		public void CloseConnection()
		{
			Logger.DebugLog( "Connection closing." );
			// No implementation.
			return;
		}

		/// <summary>
		/// Called upon the data retrieval being completed, void.
		/// </summary>
		public void CompletedDataRetrieval()
		{
			Logger.DebugLog( "Data retrieval completed." );
			// No implementation.
			return;
		}

		/// <summary>
		/// Get the column definitions from the selected item.
		/// </summary>
		/// <returns>Column definitions (name, data type).</returns>
		public IEnumerable<ColumnDefinition> GetAvailableColumns()
		{
			Logger.DebugLog( "Requesting available columns." );
			return m_item.GetColumnDefinitions();
		}

		/// <summary>
		/// Get items based on the time range and amount limitation.
		/// </summary>
		/// <returns>The items.</returns>
		public IEnumerable<DataItem> GetItems()
		{
			Logger.DebugLog( "Requesting multiple data items.\r\n" +
				"Min. range: " + RangeMinUTC.ToString() + "\r\nMax. range: " +
				m_rangeMaxUTC + "\r\nMax. items: " + m_maxItemsToRetrieve );
			return m_item.GetMultipleItems( RangeMinUTC, m_rangeMaxUTC, m_maxItemsToRetrieve );
		}

		/// <summary>
		/// Get deleted items. Not available in Severa interface.
		/// </summary>
		/// <param name="MoreResults">Always returns as False.</param>
		/// <returns>null.</returns>
		public IEnumerable<string> GetDeletedItems( out bool MoreResults )
		{
			Logger.DebugLog( "Requesting deleting items." );
			// No implementation.
			MoreResults = false;
			return new List<string>();
		}

		/// <summary>
		/// Get one item.
		/// </summary>
		/// <param name="columnOrdinalExtID">Column ordinal for external ID.</param>
		/// <param name="externalID">External ID.</param>
		/// <returns>One item.</returns>
		public DataItem GetOneItem( int columnOrdinalExtID, string externalID )
		{
			Logger.DebugLog( "Requesting item with external ID " + externalID + "." );
			return m_item.GetOneItem( externalID );
		}

		/// <summary>
		/// Interrupt. Void.
		/// </summary>
		public void Interrupt()
		{
			Logger.DebugLog( "Interruption request sent by the server." );
			// No implementation.
			return;
		}

		/// <summary>
		/// Prepare for data retrieval. Set (imaginary) SELECT statement.
		/// </summary>
		/// <param name="selectStatement">The SELECT statement.</param>
		public void PrepareForDataRetrieval( string selectStatement )
		{
			Logger.DebugLog( "Select statement set: " + selectStatement );
			m_item = ParseItemFromSelectStatement( selectStatement );
		}

		/// <summary>
		/// Set the column ordinals that should be present in the record set.
		/// </summary>
		/// <param name="columnOrdinals">Column ordinals array.</param>
		public void SetColumnsToRetrieve( int[] columnOrdinals )
		{
			Logger.DebugLog( "Column ordinals set." );
			m_item.SetColumnsToRetrieve( columnOrdinals );
		}

		/// <summary>
		/// Set the timestamp range for retrieval.
		/// </summary>
		/// <param name="SetFromDate">Minimum DateTime set?</param>
		/// <param name="FromDate">Minumim DateTime.</param>
		/// <param name="SetToDate">Maximum DateTime set?</param>
		/// <param name="ToDate">Maximum DateTime.</param>
		public void SetTimestampRangeToRetrieve( bool SetFromDate, DateTime FromDate, bool SetToDate, DateTime ToDate )
		{
			Logger.DebugLog( "Server set the time range.\r\nMin: " + FromDate.ToString() + " ("
				+ SetFromDate.ToString() + ")\r\nMax: " + ToDate.ToString() + " (" + SetToDate.ToString() + ")" );
			// If given, assign the datetime. Otherwise null.
			m_rangeMinUTC = SetFromDate ? (DateTime?) FromDate : null;
			m_rangeMaxUTC = SetToDate ? (DateTime?) ToDate : null;
		}

		/// <summary>
		/// Set the maximum amount of items to retrieve.
		/// </summary>
		/// <param name="maxItemsToRetrieve">Maximum amount.</param>
		public void SetMaxItemsToRetrieve( int maxItemsToRetrieve )
		{
			Logger.DebugLog( "Server set the maximum amount of items to " + maxItemsToRetrieve + "." );
			m_maxItemsToRetrieve = maxItemsToRetrieve;
		}

		/// <summary>
		/// Is the result set partial?
		/// </summary>
		/// <returns>A boolean value.</returns>
		public bool IsPartialResultSet()
		{
			// Is this item always complete?
			bool itemAlwaysComplete =
				( m_item != null ) ? m_item.AlwaysCompleteResultSet : true;

			// Are there any restrictions set to the result set?
			bool restricted = ( ( m_maxItemsToRetrieve != -1 ) ||
				( m_rangeMinUTC != null ) ||
				( m_rangeMaxUTC != null ) );

			// If there are restrictions and item is not always complete, this set is partial.
			bool partial = restricted && !itemAlwaysComplete;
			Logger.DebugLog( "This results set is partial: " + partial.ToString() );
			return partial;
		}

		/// <summary>
		/// Determine the queryable item from the select statement.
		/// </summary>
		/// <param name="selectStatement">The SELECT statement.</param>
		/// <returns>The Item object.</returns>
		private IItem ParseItemFromSelectStatement( string selectStatement )
		{

			// The syntax for SELECT statement in this case is defined as following:
			//
			// statement::  [item|type]=<item>;columns=<columns>
			// item     ::  contact|account|case|... (any available item)
			// columns  ::  *|<column>|<column>(,<column>)*
			// column   ::  [any object-specific column name]
			//
			// for example  :   item=contact;columns=GUID, FirstName, IsActive

			// Split the statement.
			string[] parts = selectStatement.Split( ';' );

			if( parts.Length != 2 )
			{
				throw new Exception( "Invalid select statement. ;" );
			}

			string itemname = "";
			string columns = "";

			// Parse "type" and "columns" parameters.
			for( int i = 0; i < 2; ++i )
			{
				string[] innerParts = parts[ i ].Split( '=' );
				if( innerParts.Length != 2 )
				{
					// Not recognizable as form "param=value".
					throw new Exception( "Invalid select statement. =" );
				}

				if( innerParts[ 0 ].Trim().ToLower() == "type" ||
					innerParts[ 0 ].Trim().ToLower() == "item" )
				{
					itemname = innerParts[ 1 ].Trim().ToLower();
				}

				if( innerParts[ 0 ].Trim().ToLower() == "columns" )
				{
					columns = innerParts[ 1 ].Trim().ToLower();
				}
			}

			IItem item;

			// Create new object according to item name
			switch( itemname )
			{
				case "contact":
					item = new ItemContact( m_agent );
					break;
				case "case":
					item = new ItemCase( m_agent );
					break;
				case "account":
					item = new ItemAccount( m_agent );
					break;
				case "phase":
					item = new ItemPhase( m_agent );
					break;
				case "salesprocess":
					item = new ItemSalesProcess( m_agent );
					break;
				case "salesstatus":
					item = new ItemSalesStatus( m_agent );
					break;
				case "product":
					item = new ItemProduct( m_agent );
					break;
				case "tag":
					item = new ItemTag( m_agent );
					break;
				case "businessunit":
					item = new ItemBusinessUnit( m_agent );
					break;
				case "user":
					item = new ItemUser( m_agent );
					break;
				case "worktype":
					item = new ItemWorkType( m_agent );
					break;
				case "company":
					item = new ItemCompany( m_agent );
					break;
				case "address":
					item = new ItemAddress( m_agent );
					break;
				case "product category":
				case "productcategory":
					item = new ItemProductCategory( m_agent );
					break;
				case "industry":
					item = new ItemIndustry( m_agent );
					break;
				case "invoice":
					item = new ItemInvoice( m_agent );
					break;
				case "timezone":
					item = new ItemTimezone( m_agent );
					break;
				case "pricelist":
					item = new ItemPricelist( m_agent );
					break;
				case "language":
					item = new ItemLanguage( m_agent );
					break;
				case "lead source":
				case "leadsource":
					item = new ItemLeadSource( m_agent );
					break;
				default:
					throw new Exception( "Invalid select statement (item \"" + itemname + "\" not recognized)." );
			}

			item.SetColumnsForQuery( columns );
			return item;

		}

		/// <summary>
		/// Parse a value from the connection string.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="source">The complete connection string.</param>
		/// <returns>The value.</returns>
		private string GetValue( string name, string source )
		{
			// APIKEY differs a bit from the other values.
			// It can contain basically any characters, so we can't parse it normally.
			// Instead, we just read 32 characters straight ahead from wherever the
			// "apikey" substring occurs in the connection string.
			if( name == "apikey" )
			{
				try
				{
					return source.Substring( source.ToLower().IndexOf( "apikey=" ) + 7, 32 );
				}
				catch
				{
					return "";
				}
			}

			// Otherwise, split the connection string to pieces by semicolon and find 
			// the "name=value" pair by the beginning "name=".
			string[] parts = source.Split( ';' );
			foreach( string part in parts )
			{
				if( part.Length > name.Length + 1 )
				{
					if( part.ToLower().StartsWith( name + "=" ) )
					{
						return part.Substring( name.Length + 1, part.Length - name.Length - 1 );
					}
				}
			}
			// If nothing was found, return an empty string.
			return "";
		}



	}
}
