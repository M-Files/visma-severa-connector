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
	/// A class to retrieve and map Case item from Severa to M-Files.
	/// </summary>
	class ItemCase : Item, IItem
	{

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemCase(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{{ "GUID", "System.String" },             // Case.GUID
                                               { "Name", "System.String" },
											   { "AccountGUID", "System.String" },
											   { "CaseNumber", "System.Int32" },
											   { "ClosedDate", "System.DateTime" },
											   { "ContactGUID", "System.String" },
											   { "CurrentCaseStatusDesc", "System.String" },
											   { "DeadlineDate", "System.DateTime" },
											   { "Description", "System.String" },
											   { "ExpectedValue", "System.Double" },
											   { "IsClosed", "System.Boolean" },
											   { "IsInternal", "System.Boolean" },
											   { "OrderNumber", "System.String" },
											   { "OurReference", "System.String" },
											   { "Priority", "System.Int32" },
											   { "Probability", "System.Int32" },
											   { "SalesClosedDate", "System.DateTime" },
											   { "StartDate", "System.DateTime" },
											   { "SalesProcessGUID", "System.String" },
											   { "BusinessUnitGUID", "System.String" },
											   { "SalesStatusGUID", "System.String" },
											   { "CaseOwnerUserGUID", "System.String" },
											   { "CostCenterGUID", "System.String" },
											   { "SalesPersonUserGUID", "System.String" },
											   { "LeadSourceGUID", "System.String" },
											   { "CaseMembers", "System.String" } };
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

			// Some cases deleted in Severa were not removed from M-Files.
			// Changed this to get all cases
			Case[] cases = m_agent.GetAllCases();
			foreach( Case _case in cases )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Yield return data items and increase count.
				if( !maxRangeSpecified || MatchesMaxRange( _case, maxRange ) )
				{
					// Get Users (Case members) for phase.
					User[] users = m_agent.GetCaseMembers( _case.GUID );

					// First data item with possible member
					DataItem caseItem = FormDataItem( _case, users.Count() > 0 ? users[ 0 ] : null );
					yield return caseItem;
					recordCount++;

					// Make new instances of data items if there is more than one member.
					// These are not counted against MaxResults
					for( int i = 1; i < users.Count(); ++i )
					{
						DataItem newCaseItem = FormDataItem( _case, users[ i ] );
						yield return newCaseItem;
					}
				}
			}
		}

		/// <summary>
		/// Compares a record modification date to max range date if specified.
		/// </summary>
		/// <param name="_case">Case object.</param>
		/// <param name="MaxRange">Max date range.</param>
		/// <returns>Comparison result: does the date fit the max range?</returns>
		private bool MatchesMaxRange( Case _case, DateTime MaxRange )
		{
			// TODO: This can be reenabled in future if necessary, but since it is
			// not obligatory to limit the results, to *just* to the given range,
			// this comparison will just merely slow down the plugin.
			return true;

			// No last modified info available.
			/* return true; */
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Case GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOneCase( GUID ), null );
		}

		/// <summary>
		/// Converts an case into a data item.
		/// </summary>
		/// <param name="Case">Case object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Case _case, User caseMember )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				// Case member is a special case
				if( selectedColumns[ retrievedColumns[ i ] ] == AVAILABLE_COLUMNS[ 25, 0 ] )
					values.Add( retrievedColumns[ i ], caseMember != null ? caseMember.GUID : null );
				else
					values.Add( retrievedColumns[ i ], GetValueFromObject( _case, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Case">Case object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Case _case, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return _case.GUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return _case.Name;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return _case.AccountGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return _case.CaseNumber;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return _case.ClosedDate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return _case.ContactGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return _case.CurrentCaseStatusDescription;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return _case.DeadlineDate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return _case.Description;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
			{
				return _case.ExpectedValue;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 10, 0 ] )
			{
				return _case.IsClosed;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 11, 0 ] )
			{
				return _case.IsInternal;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 12, 0 ] )
			{
				return _case.OrderNumber;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 13, 0 ] )
			{
				return _case.OurReference;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 14, 0 ] )
			{
				return _case.Priority;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 15, 0 ] )
			{
				return _case.Probability;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 16, 0 ] )
			{
				return _case.SalesCloseDate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 17, 0 ] )
			{
				return _case.StartDate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 18, 0 ] )
			{
				return _case.SalesProcessGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 19, 0 ] )
			{
				return _case.BusinessUnitGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 20, 0 ] )
			{
				return _case.SalesStatusGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 21, 0 ] )
			{
				return _case.CaseOwnerUserGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 22, 0 ] )
				return _case.CostCenterGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 23, 0 ] )
				return _case.SalesPersonUserGUID;

			if( ColumnName == AVAILABLE_COLUMNS[ 24, 0 ] )
				return _case.LeadSourceGUID;

			throw new Exception( "Column " + ColumnName + " not found." );

		}
	}
}
