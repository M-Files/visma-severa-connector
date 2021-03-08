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
	/// A class to retrieve and map Contact item from Severa to M-Files.
	/// </summary>
	class ItemContact : Item, IItem
	{

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemContact(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,] {{"GUID", "System.String"},
											   {"FirstName", "System.String"},
											   {"LastName", "System.String"},
											   {"JobTitle", "System.String"},
											   {"AccountGUID", "System.String"},
											   {"FullName", "System.String"},
											   {"IsActive", "System.Boolean"},
											   {"SatisfactionLevel", "System.String"},
											   {"Salutation", "System.String"},
											   {"Description", "System.String"},
											   {"CommunicationMethods", "System.String"},
											   {"EmailAddress", "System.String" },
											   {"MobilePhone", "System.String" },
											   {"Phone", "System.String" },
											   {"Fax", "System.String" },
											   {"InstantMessenger", "System.String" },
											   {"IPPhone", "System.String" },
											   {"Keywords", "System.String" },
											   {"ContactRoleGUID", "System.String" },
											   {"AssressGUID", "System.String" },
											   {"LanguageGUID", "System.String" },
											   {"DateOfBirth", "System.DateTime" }};
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

			// Get the contacts based on the range minimum.
			Contact[] contacts = m_agent.GetModifiedContacts( RangeMinUTC );
			foreach( Contact contact in contacts )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( contact, maxRange ) )
				{
					yield return FormDataItem( contact );
					recordCount++;
				}

			}
		}


		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="contact">Contact object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Contact contact, DateTime MaxRange )
		{
			// No last modified information available, always true.
			return true;
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Contact GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneContact( GUID ) );
		}

		/// <summary>
		/// Converts an contact into a data item.
		/// </summary>
		/// <param name="Contact">Contact object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with contact data.</returns>
		private DataItemSimple FormDataItem( Contact Contact )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				values.Add( retrievedColumns[ i ], GetValueFromObject( Contact, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Contact">Contact object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Contact Contact, string ColumnName )
		{

			// GUID
			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return Contact.GUID;
			}

			// First name
			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return Contact.FirstName;
			}

			// Last name
			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return Contact.LastName;
			}

			// Job title
			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return Contact.JobTitle;
			}

			// Account GUID
			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return Contact.AccountGUID;
			}

			// FullName
			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return Contact.FirstName + " " + Contact.LastName;
			}

			// Is active
			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return Contact.IsActive;
			}

			// Satisfaction level
			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return Contact.SatisfactionLevel;
			}

			// Salutation
			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return Contact.Salutation;
			}

			// Description
			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
			{
				return Contact.Description;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 10, 0 ] )
			{
				return GetCommunicationMethodsAsString( Contact );
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 11, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "emailaddress" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 12, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "mobilephone" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 13, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "phone" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 14, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "fax" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 15, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "instantmessenger" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 16, 0 ] )
			{
				foreach( Contact.CommunicationMethod method in Contact.CommunicationMethods )
				{
					if( method.Type.ToLower().Equals( "ipphone" ) )
						return method.Value;
				}
				return "";
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 17, 0 ] )
			{
				string words = "";
				for( int i = 0; i < Contact.Keywords.Count(); ++i )
				{
					words += Contact.Keywords[ i ] + " ";
				}
				return words;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 18, 0 ] )
				return Contact.ContactRoleGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 19, 0 ] )
				return Contact.AddressGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 20, 0 ] )
				return Contact.LanguageGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 21, 0 ] )
				return Contact.DateOfBirth;

			throw new Exception( "Column " + ColumnName + " not found." );

		}

		/// <summary>
		/// Get communication methods collection as one string.
		/// </summary>
		/// <param name="Contact">Contact object</param>
		/// <returns>A list of communication methods as a string.</returns>
		private string GetCommunicationMethodsAsString( Contact Contact )
		{
			string value = "";

			// Loop through the communication methods
			foreach( Contact.CommunicationMethod cm in Contact.CommunicationMethods )
			{
				// If it's not forbidden to use this method, add the value to the string.
				if( !cm.IsForbiddenToUse )
					value += cm.Name + " (" + cm.Type + "): " + cm.Value + "\r\n";
			}

			return value;
		}

	}
}
