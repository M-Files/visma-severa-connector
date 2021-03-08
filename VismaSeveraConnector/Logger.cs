/*

This code is provided as a reference sample only and has no explicit or implicit support
as to its nature, completeness, nor function.  Please see the license file
(included in this repository) for more details.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VismaSeveraConnector
{
	/// <summary>
	/// A static class for logging.
	/// </summary>
	public static class Logger
	{
		private static bool m_debugLog = false;
		private static string m_debugLogFile = "";

		/// <summary>
		/// Turn on debugging messages.
		/// </summary>
		/// <param name="File"></param>
		public static void TurnOnDebugLogging( string File ) 
		{
			m_debugLogFile = File;
			m_debugLog = true;
		}

		/// <summary>
		/// Write a message to debug log.
		/// </summary>
		/// <param name="Message">Messae.</param>
		public static void DebugLog( string Message )
		{
			// If debug logging is off, return now.
			if( !m_debugLog )
				return;

			// Open and append the log file.
			System.IO.StreamWriter sw = new System.IO.StreamWriter( m_debugLogFile, true );
			sw.WriteLine( DateTime.Now.ToString() + " : " + Message );
			sw.Close();
		}
	}
}
