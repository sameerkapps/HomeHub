/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;

namespace HomeHubApp.Authenticator
{
    /// <summary>
    /// Argument for ADAuthentication exception
    /// </summary>
    public class ADAuthenticatorEventArgs : EventArgs
    {
        /// <summary>
        /// Error that occured
        /// </summary>
        public ADAuthenticationResult Error { get; }

        /// <summary>
        /// Message string for the exception
        /// </summary>
        public string Message { get; }

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
