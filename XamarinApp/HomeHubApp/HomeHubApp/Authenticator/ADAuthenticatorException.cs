/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;

namespace HomeHubApp.Authenticator
{
    /// <summary>
    /// Exception for AD Authentication
    /// Store error enum in the class and
    /// message in the base
    /// </summary>
    public class ADAuthenticatorException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public ADAuthenticatorException(object sender, ADAuthenticatorEventArgs args)
            :base(args.Message)
        {
            Error = args.Error;
        }

        /// <summary>
        /// Error that caused exception
        /// </summary>
        public ADAuthenticationResult Error { get; }
    }
}
