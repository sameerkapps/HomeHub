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

using Android.App;
using Xamarin.Forms;
using HomeHubApp.Authenticator;

namespace HomeHubApp.Authenticator
{
    /// <summary>
    /// This class is for performing authentication using Azure Active directory
    /// </summary>
    public class ADAuthenticator : IADAuthenticator
    {
        #region Accessors
        /// <summary>
        /// If user has signed in or not
        /// </summary>
        public bool IsSignedIn { get; private set; }
        #endregion

        /// <summary>
        /// Configure with the required parameters
        /// </summary>
        /// <param name="aadInstance">https://login.microsoftonline.com/{0}</param>
        /// <param name="tenant">contoso.onmicrosoft.com</param>
        /// <param name="clientId">Your client GUID</param>
        /// <param name="redirectUrl">http://YourService</param>
        /// <param name="homeHumResourceId">https://contoso.onmicrosoft.com/YourService</param>
        public void Configure(string aadInstance,
                              string tenant,
                              string clientId,
                              string redirectUrl,
                              string homeHumResourceId)
        {
            _aadInstance = aadInstance;
            _tenant = tenant;
            _clientId = clientId;
            _redirectUri = new Uri(redirectUrl);
            _homeHubResourceId = homeHumResourceId;

            _authority = String.Format(CultureInfo.InvariantCulture, _aadInstance, _tenant);
            _authContext = new AuthenticationContext(_authority, new TokenCache());
        }

    /// <summary>
    /// Performs sign in
    /// </summary>
    /// <returns></returns>
    public async Task<ADAuthenticationResult> SignIn()
        {
            //
            // Get an access token to call the Home Hub service.
            //
            AuthenticationResult result = null;
            try
            {
                // acquire the token through prompt
                result = await _authContext.AcquireTokenAsync(_homeHubResourceId, 
                                                              _clientId, 
                                                              _redirectUri, 
                                                              new PlatformParameters((Activity)Forms.Context));
                // set the internal flag
                IsSignedIn = true;

                // return success
                return ADAuthenticationResult.Success;
            }
            catch (AdalException ex)
            {
                // if user pressed cancel
                // throw appropriate exception
                //if (ex.ErrorCode == AdalError. "authentication_canceled")
                if(ex.ErrorCode == AdalError.AuthenticationCanceled)
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
        public async Task<string> GetToken()
        {
            //
            // Get an access token to call the Home Hub service.
            //
            AuthenticationResult result = null;
            IsSignedIn = false;

            try
            {
                // attempt the silent acquisition
                result = await _authContext.AcquireTokenSilentAsync(_homeHubResourceId, _clientId);
                // set the flag
                IsSignedIn = true;

                // return the token
                return result.AccessToken;
            }
            catch (AdalException ex)
            {
                // if user interaction i.e. sign in is required,
                // throw appropriate exception
                if (ex.ErrorCode == AdalError.UserInteractionRequired ||
                    ex.ErrorCode == AdalError.FailedToAcquireTokenSilently)
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
        }

        #region private members
        private static string _aadInstance;
        private static string _tenant;
        private static string _clientId;
        Uri _redirectUri;

        //
        // To authenticate to the Home Hub service, the client needs to know the service's App ID URI.
        // To contact the Home Hub service we need it's URL as well.
        //
        private static string _homeHubResourceId;

        private static string _authority;

        private AuthenticationContext _authContext = null;
        #endregion
    }
}
