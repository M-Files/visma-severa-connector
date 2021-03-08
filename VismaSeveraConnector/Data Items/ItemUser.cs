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
    class ItemUser : Item, IItem
    {
		/// <summary>
		/// Constructor. Calls the base class constructor and initialized the column array.
		/// </summary>
		/// <param name="Agent">VSInterfaceAgent object.</param>
		public ItemUser(
			VSInterfaceAgent Agent
			)
			: base( Agent )
		{
			AVAILABLE_COLUMNS = new string[ , ]{{ "GUID", "System.String" },             // User.GUID
                                               { "FullName", "System.String" }, 
                                               { "FirstName", "System.String" },
                                               { "LastName", "System.String" },
                                               { "Title", "System.String" },
                                               { "Email", "System.String" },
                                               { "Phone", "System.String" },
                                               { "BusinessUnitGUID", "System.String" },
                                               { "SuperiorUserGUID", "System.String" },
                                               { "Notes", "System.String" },
                                               { "Code", "System.String" },
                                               { "IsActive", "System.Boolean"}};
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

			// Get users based on the timestamp
            User[] users = m_agent.GetModifiedUsers(RangeMinUTC);
            foreach (User _user in users)
            {
				// Break if max result count reached.
				if( recordCount > 0 && recordCount == MaxResults )
					break;

                yield return FormDataItem(_user);
				recordCount++;
            }
        }

		/// <summary>
		/// Get one data item.
		/// </summary>
		/// <param name="GUID">User GUID.</param>
		/// <returns>Data item created from the GUID.</returns>
        public override DataItem GetOneItem(string GUID)
        {
            return FormDataItem(m_agent.GetOneUser(GUID));
        }

		/// <summary>
		/// Converts an user into a data item.
		/// </summary>
		/// <param name="_user">User object.</param>
		/// <returns>MFiles.Server.Extensions.DataItemSimple with user data.</returns>
        private DataItemSimple FormDataItem(User _user)
        {
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < retrievedColumns.Length; ++i)
            {
                values.Add(retrievedColumns[i], GetValueFromObject(_user, selectedColumns[retrievedColumns[i]]));
            }
            return new DataItemSimple(values);
        }

		/// <summary>
		/// Extracts a value from the object.
		/// </summary>
		/// <param name="Account">User object.</param>
		/// <param name="ColumnName">Column name.</param>
		/// <returns>A value of undefined type.</returns>
        private Object GetValueFromObject(User _user, string ColumnName)
        {

            if (ColumnName == AVAILABLE_COLUMNS[0, 0])
            {
                return _user.GUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[1, 0])
            {
                return _user.FirstName + " " + _user.LastName;
            }

            if (ColumnName == AVAILABLE_COLUMNS[2, 0])
            {
                return _user.FirstName;
            }

            if (ColumnName == AVAILABLE_COLUMNS[3, 0])
            {
                return _user.LastName;
            }

            if (ColumnName == AVAILABLE_COLUMNS[4, 0])
            {
                return _user.Title;
            }

            if (ColumnName == AVAILABLE_COLUMNS[5, 0])
            {
                return _user.Email;
            }

            if (ColumnName == AVAILABLE_COLUMNS[6, 0])
            {
                return _user.Phone;
            }

            if (ColumnName == AVAILABLE_COLUMNS[7, 0])
            {
                return _user.BusinessUnitGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[8, 0])
            {
                return _user.SuperiorUserGUID;
            }

            if (ColumnName == AVAILABLE_COLUMNS[9, 0])
            {
                return _user.Notes;
            }

            if (ColumnName == AVAILABLE_COLUMNS[10, 0])
            {
                return _user.Code;
            }

            if (ColumnName == AVAILABLE_COLUMNS[11, 0])
            {
                return _user.IsActive;
            }

            throw new Exception("Column " + ColumnName + " not found.");

        }

    }
}
