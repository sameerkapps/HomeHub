/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;

using HomeHubAuthenticator;

namespace HomeHubWPFClient.Authenticator
{
    /// <summary>
    /// Argument for ADAuthentication exception
    /// </summary>
    internal class ADAuthenticatorEventArgs : EventArgs
    {
        /// <summary>
        /// Error that occured
        /// </summary>
        internal ADAuthenticationResult Error { get; }

        /// <summary>
        /// Message string for the exception
        /// </summary>
        internal string Message { get; }

        /// <summary>
        /// Contstructor
        /// </summary>
        /// <param name="error">Error enum</param>
        /// <param name="message">message</param>
        public ADAuthenticatorEventArgs(ADAuthenticationResult error, string message)
        {
            Error = error;
            Message = message;
        }
    }
}
