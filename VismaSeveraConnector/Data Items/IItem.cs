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
	/// Interface for items that are retrievable from the Severa web service.
	/// </summary>
    interface IItem
    {
		bool AlwaysCompleteResultSet { get; }
        void SetColumnsForQuery(string Columns);
        void SetColumnsToRetrieve(int[] ColumnOrdinals);
        IEnumerable<DataItem> GetMultipleItems( DateTime RangeMinUTC, DateTime? RangeMaxUTC, int MaxResults);
        DataItem GetOneItem(string ExternalID);
        IEnumerable<ColumnDefinition> GetColumnDefinitions();
    }
}
