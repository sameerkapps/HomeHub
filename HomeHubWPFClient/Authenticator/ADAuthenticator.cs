/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * Part of this code is from one of the Microsoft samples
 * Their copyright notice is left here
***********************************************************************************************/
//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------
using System;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

using HomeHubWPFClient.Authenticator;

namespace HomeHubAuthenticator
{
    /// <summary>
    /// This class is for performing authentication using Azure Active directory
    /// </summary>
    public class ADAuthenticator
    {
        /// <summary>
        /// Constructor to initialize the context.
        /// If there is only user in the client app (which is generally the case,
        /// this should be singleton. I did not have time to do it
        /// </summary>
        public ADAuthenticator()
        {
            _authority = String.Format(CultureInfo.InvariantCulture, _aadInstance, _tenant);
            _authContext = new AuthenticationContext(_authority, new TokenCache());
        }

        #region Accessors
        /// <summary>
        /// If user has signed in or not
        /// </summary>
        public bool IsSignedIn { get; private set; }
        #endregion

        /// <summary>
        /// Performs sign in
        /// </summary>
        /// <returns></returns>
        internal async Task<ADAuthenticationResult> SignIn()
        {
            //
            // Get an access token to call the Home Hub service.
            //
            AuthenticationResult result = null;
            try
            {
                // acquire the token through prompt
                result = await _authContext.AcquireTokenAsync(_homeHubResourceId, _clientId, _redirectUri, new PlatformParameters(PromptBehavior.Always));
                // set the internal flag
                IsSignedIn = true;

                // return success
                return ADAuthenticationResult.Success;
            }
            catch (AdalException ex)
            {
                // if user pressed cancel
                // throw appropriate exception
                if (ex.ErrorCode == "authentication_canceled")
                {
                    throw new ADAuthenticatorException(this, new ADAuthenticatorEventArgs(ADAuthenticationResult.SignInCancelled, ex.ErrorCode));
                }
                else
                {
                    // if the error is different
                    // throw different exception
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    throw new ADAuthenticatorException(this, new ADAuthenticatorEventArgs(ADAuthenticationResult.AdalError, message));
                }
            }
        }

        /// <summary>
        /// Gets the token from the context
        /// </summary>
        /// <returns></returns>
        internal async Task<string> GetToken()
        {
            //
            // Get an access token to call the Home Hub service.
            //
            AuthenticationResult result = null;
            IsSignedIn = false;

            try
            {
                // attempt the silent acquisition
                result = await _authContext.AcquireTokenAsync(_homeHubResourceId, _clientId, _redirectUri, new PlatformParameters(PromptBehavior.Never));
                // set the flag
                IsSignedIn = true;

                // return the token
                return result.AccessToken;
            }
            catch (AdalException ex)
            {
                // if user interaction i.e. sign in is required,
                // throw appropriate exception
                if (ex.ErrorCode == "user_interaction_required")
                {
                    throw new ADAuthenticatorException(this, new ADAuthenticatorEventArgs(ADAuthenticationResult.SignInRequired, ex.ErrorCode));
                }
                else
                {
                    // if the error is different
                    // throw different exception
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    throw new ADAuthenticatorException(this, new ADAuthenticatorEventArgs(ADAuthenticationResult.AdalError, message));
                }
            }
        }

        /// <summary>
        /// Perform sign out
        /// </summary>
        public void SignOut()
        {
            // no need to state check. Just sign out.
            IsSignedIn = false;
            // clear the cache
            _authContext.TokenCache.Clear();
            // Also clear cookies from the browser control.
            ClearCookies();
        }

        #region private methods
        // This function clears cookies from the browser control used by ADAL.
        private void ClearCookies()
        {
            const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        #endregion

        #region private members
        private static string _aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string _tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string _clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        Uri _redirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);

        //
        // To authenticate to the Home Hub service, the client needs to know the service's App ID URI.
        // To contact the Home Hub service we need it's URL as well.
        //
        private static string _homeHubResourceId = ConfigurationManager.AppSettings["homehub:HomeHubResourceId"];

        private static string _authority;

        private AuthenticationContext _authContext = null;
        #endregion
    }
}
