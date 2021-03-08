/*

This code is provided as a reference sample only and has no explicit or implicit support
as to its nature, completeness, nor function.  Please see the license file
(included in this repository) for more details.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VismaSeveraConnector
{
    /// <summary>
    /// A class for certificate handling.
    /// </summary>
    public static class Certifier
    {
		/// <summary>
		/// Certificate.
		/// </summary>
        private static X509Certificate2 cert;

		/// <summary>
		/// *** WARNING! *** 
		/// This should not be used unless you're absolutely sure about what you doing.
		/// The option accepts any certificate at all.
		/// </summary>
		public static void SetUnsafePolicy()
		{
			ServicePointManager.ServerCertificateValidationCallback += AlwaysAccept;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        private static string GetDir()
        {
            string executingDll = Assembly.GetExecutingAssembly().Location;
            string[] pathparts = executingDll.Split('\\');
            string myDir = "";
            for (int i = 0; i < pathparts.Length - 1; ++i)
            {
                myDir += pathparts[i] + "\\";
            }
            return myDir;
        }

		/// <summary>
		/// Validation function (event handler)
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="cert">Certificate.</param>
		/// <param name="chain">Chain.</param>
		/// <param name="error">Possible Errors.</param>
		/// <returns></returns>
		private static bool AlwaysAccept(
			object sender,
			X509Certificate cert,
			X509Chain chain,
			SslPolicyErrors error
			)
		{
			return true;
		}

		/// <summary>
		/// Validation function (event handler)
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="cert">Certificate.</param>
		/// <param name="chain">Chain.</param>
		/// <param name="error">Possible Errors.</param>
		/// <returns></returns>
        private static bool RemoteCertificateValidate(object sender,
            X509Certificate cert,
            X509Chain chain,
            SslPolicyErrors error)
        {
            return true;
        }
    }
}