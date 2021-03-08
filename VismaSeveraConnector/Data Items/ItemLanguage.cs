/*

This code is provided as a reference sample only and has no explicit or implicit support
as to its nature, completeness, nor function.  Please see the license file
(included in this repository) for more details.

*/

using MFiles.Server.Extensions;
using Severa.Entities.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VismaSeveraConnector
{
	/// <summary>
	/// A class to retrieve and map Language item from Severa to M-Files.
	/// </summary>
	class ItemLanguage : Item, IItem
	{
		/// <summary>		
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemLanguage(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,] { { "GUIDField", "System.String" },
												{ "IetfLanguageTagField", "System.String" },
												{ "Name", "System.String" } };
		}

		/// <summary>
		/// Retrieve multiple items from the Web Service.
		/// </summary>      
		/// <param name="MaxResults">Max amount of results returned.</param>
		/// <param name="RangeMaxUTC">Minimum range for "Modified since".</param>
		/// <param name="RangeMinUTC">Maximum range for "Modified since" or NULL.</param>
		/// <returns>A collection of Language data items.</returns>
		public override IEnumerable<DataItem> GetMultipleItems(
			DateTime RangeMinUTC,
			DateTime? RangeMaxUTC,
			int MaxResults )
		{
			int recordCount = 0;

			IEnumerable<Language> languages = m_agent.GetAllLanguages();
			if( languages == null )
				yield break;
			foreach( Language _language in languages )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				yield return FormDataItem( _language );
				recordCount++;
			}
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Language GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneLanguage( GUID ) );
		}



		/// <summary>
		/// Converts an company into a data item.
		/// </summary>
		/// <param name="_language">Language object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Language _language )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( _language, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_timezone">Language object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Language _language, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
				return _language.GUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
				return _language.IetfLanguageTag;

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
				return _language.Name;

			throw new Exception( "Column " + ColumnName + " not found." );

		}
	}
}
