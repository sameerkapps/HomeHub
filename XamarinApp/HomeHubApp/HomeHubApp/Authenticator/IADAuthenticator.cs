/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHubApp.Authenticator
{
    /// <summary>
    /// Interface to sign-in/out and retrieve token from cache
    /// </summary>
    public interface IADAuthenticator
    {
        #region Accessors
        /// <summary>
        /// If user has signed in or not
        /// </summary>
        bool IsSignedIn { get; }
        #endregion

        #region methods
        /// <summary>
        /// Configure with the required parameters
        /// </summary>
        /// <param name="aadInstance">https://login.microsoftonline.com/{0}</param>
        /// <param name="tenant">contoso.onmicrosoft.com</param>
        /// <param name="clientId">Your client GUID</param>
        /// <param name="redirectUrl">http://YourService</param>
        /// <param name="homeHubResourceId">https://contoso.onmicrosoft.com/YourService</param>
        void Configure(string aadInstance,
                              string tenant,
                              string clientId,
                              string redirectUrl,
                              string homeHubResourceId);

        /// <summary>
        /// Performs sign in
        /// </summary>
        /// <returns></returns>
        Task<ADAuthenticationResult> SignIn();

        /// <summary>
        /// Gets the token from the context
        /// </summary>
        /// <returns></returns>
        Task<string> GetToken();

        /// <summary>
        /// Perform sign out
        /// </summary>
        void SignOut();
        #endregion
    }
}
