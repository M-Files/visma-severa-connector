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
	/// Base class for items in Severa.
	/// </summary>
	 class Item
	{
		protected string[ , ] AVAILABLE_COLUMNS = new string[ 0, 0 ];
		protected int COLUMNS_COUNT { get { return AVAILABLE_COLUMNS.GetLength( 0 ); } }

		protected VSInterfaceAgent m_agent;
		protected string[] selectedColumns;
		protected int[] retrievedColumns;

		/// <summary>
		/// This property indicates if the item does not support partial result sets (by date range).
		/// </summary>
		public virtual bool AlwaysCompleteResultSet { get { return false; } }

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected Item()
		{
		}

		/// <summary>
		/// A common constructor for all Items.
		/// </summary>
		/// <param name="Agent">A VSInterfaceAgent object.</param>
		protected Item( 
			VSInterfaceAgent Agent 
			)
		{
			m_agent = Agent;
		}


		/// <summary>
		/// Select columns to query.
		/// </summary>
		/// <param name="Columns">Column names as a string array.</param>
		public void SetColumnsForQuery( string Columns )
		{
			selectedColumns = GetColumnsArray( Columns );
		}

		/// <summary>
		/// Select column ordinals to retrieve.
		/// </summary>
		/// <param name="ColumnOrdinals">Array of column </param>
		public void SetColumnsToRetrieve( int[] ColumnOrdinals )
		{
			retrievedColumns = ColumnOrdinals;
		}

		/// <summary>
		/// Returns true if this data source is capable of returning a single item based on its ExtID.
		/// </summary>
		/// <returns>True if GetOneItem is supported.</returns>
		public virtual bool CanGetOneItem()
		{
			// Always true.
			return true;
		}


		/// <summary>
		/// Gets the column definitions.
		/// </summary>
		/// <returns>Column names + data types.</returns>
		public IEnumerable<ColumnDefinition> GetColumnDefinitions()
		{
			List<ColumnDefinition> listDefs = new List<ColumnDefinition>();
			foreach( string colName in selectedColumns )
			{
				for( int i = 0; i < COLUMNS_COUNT; ++i )
				{
					if( colName.Trim().ToLower() == AVAILABLE_COLUMNS[ i, 0 ].Trim().ToLower() )
					{
						ColumnDefinition colDef = new ColumnDefinition();
						colDef.Name = AVAILABLE_COLUMNS[ i, 0 ];
						colDef.Ordinal = i;
						colDef.Type = System.Type.GetType( AVAILABLE_COLUMNS[ i, 1 ] );
						listDefs.Add( colDef );
						break;
					}
				}
			}
			return listDefs;
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Item GUID.</param>
		/// <returns>Data item.</returns>
		public virtual DataItem GetOneItem( string GUID )
		{
			// Virtual, return nothing.
			return null;
		}

		/// <summary>
		/// Get multiple items with a time range and maximum result set size.
		/// </summary>
		/// <param name="RangeMinUTC">Minimum date.</param>
		/// <param name="RangeMaxUTC">Maximum date.</param>
		/// <param name="MaxResults">Maximum amount of results.</param>			
		/// <returns>Collection of data items.</returns>
		public virtual IEnumerable<DataItem> GetMultipleItems(
			DateTime RangeMinUTC, 
			DateTime? RangeMaxUTC, 
			int MaxResults )
		{
			// Virtual, return nothing.
			return null;
		}

		/// <summary>
		/// Get an array of selected column names.
		/// </summary>
		/// <param name="Columns">A string defining the columns like in SQL SELECT.
		/// An asterisk (*) means that all columns are selected.
		/// Otherwise, the existence of the columns is confirmed an the column names
		/// are listed in the array in the order of their appearance in the input string.</param>
		/// <returns>An array of valid column names.</returns>
		protected string[] GetColumnsArray( string Columns )
		{

			List<string> myColumns = new List<string>();

			// Map all columns if asterisk notation is given.
			if( Columns.Trim() == "*" )
			{
				for( int i = 0; i < COLUMNS_COUNT; ++i )
				{
					myColumns.Add( AVAILABLE_COLUMNS[ i, 0 ] );
				}
				return myColumns.ToArray();
			}

			// Split the columns list.
			string[] columnNames = Columns.Split( ',' );

			// Loop through trimmed column names specified in string
			// and compare them to the AVAILABLE_COLUMNS array.
			foreach( string columnName in columnNames )
			{
				for( int i = 0; i < COLUMNS_COUNT; ++i )
				{
					// A name was found, continue with the next one.
					if( columnName.Trim().ToLower() == AVAILABLE_COLUMNS[ i, 0 ].Trim().ToLower() )
					{
						myColumns.Add( AVAILABLE_COLUMNS[ i, 0 ] );
						break;
					}

					// We reached the end of list and still no match.
					if( i == COLUMNS_COUNT - 1 )
					{
						throw new Exception( "Property \"" + columnName.Trim() + "\" does not exist in current context." );
					}

				}

			}

			// Return the columns.
			return myColumns.ToArray();
		}


	}
}
