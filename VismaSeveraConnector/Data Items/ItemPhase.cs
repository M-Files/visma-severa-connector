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
	/// A class to retrieve and map Phase item from Severa to M-Files.
	/// </summary>
	class ItemPhase : Item, IItem
	{

		/// <summary>
		/// This item always returns a complete result set.
		/// </summary>
		public override bool AlwaysCompleteResultSet
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemPhase(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[,]{{ "GUID", "System.String" },             // Case.GUID
                                               { "Name", "System.String" },
											   { "CaseGUID", "System.String" },
											   { "IsCompleted", "System.Boolean" },
											   { "OwnerUserGUID", "System.String" },
											   { "PricePerHour", "System.Double" },
											   { "WorkEstimate", "System.Double" },
											   { "WorkTypeGUID", "System.String" },
											   { "PlannedStartDate", "System.DateTime"},
											   { "Deadline", "System.DateTime" },
											   { "ParentPhaseGUID", "System.String" },
											   { "PhaseMembers", "System.String" } };
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

			// Get ALL the phases. Use the common minimum, we can't
			// define a range for phrases reliably.
			Phase[] phases = m_agent.GetAllPhases();

			foreach( Phase _phase in phases )
			{
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

				// Get Users (Phase members) for phase.
				User[] users = m_agent.GetPhaseMembers( _phase.GUID );

				// First data item with possible member
				DataItem phaseItem = FormDataItem( _phase, users.Count() > 0 ? users[ 0 ] : null );
				yield return phaseItem;
				recordCount++;

				// Make new instances of data items if there is more than one member.
				for( int i = 1; i < users.Count(); ++i )
				{
					DataItem newPhaseItem = FormDataItem( _phase, users[ i ] );
					yield return newPhaseItem;
					recordCount++;
				}
			}
		}

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">Phase GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
		public override DataItem GetOneItem( string GUID )
		{
			return FormDataItem( m_agent.GetOnePhase( GUID ), null );
		}

		/// <summary>
		/// Converts a phase into a data item.
		/// </summary>
		/// <param name="_phase">Phase object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with account data.</returns>
		private DataItemSimple FormDataItem( Phase _phase, User phaseMember )
		{
			Dictionary<int, object> values = new Dictionary<int, object>();
			for( int i = 0; i < retrievedColumns.Length; ++i )
			{
				// Phase member is a special case
				if( selectedColumns[ retrievedColumns[ i ] ] == AVAILABLE_COLUMNS[ 11, 0 ] )
					values.Add( retrievedColumns[ i ], phaseMember != null ? phaseMember.GUID : null );
				else
					values.Add( retrievedColumns[ i ], GetValueFromObject( _phase, selectedColumns[ retrievedColumns[ i ] ] ) );
			}
			return new DataItemSimple( values );
		}

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="_phase">Phase object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
		private Object GetValueFromObject( Phase _phase, string ColumnName )
		{

			if( ColumnName == AVAILABLE_COLUMNS[ 0, 0 ] )
			{
				return _phase.GUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 1, 0 ] )
			{
				return _phase.Name;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 2, 0 ] )
			{
				return _phase.CaseGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 3, 0 ] )
			{
				return _phase.IsCompleted;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 4, 0 ] )
			{
				return _phase.OwnerUserGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 5, 0 ] )
			{
				return _phase.PricePerHour;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 6, 0 ] )
			{
				return _phase.WorkEstimate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 7, 0 ] )
			{
				return _phase.WorkTypeGUID;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 8, 0 ] )
			{
				return _phase.PlannedStartDate;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 9, 0 ] )
			{
				return _phase.Deadline;
			}

			if( ColumnName == AVAILABLE_COLUMNS[ 10, 0 ] )
				return _phase.ParentPhaseGUID;

			throw new Exception( "Column " + ColumnName + " not found." );

		}

	}
}
