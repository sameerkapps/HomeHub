/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HomeHubClient
{
    /// <summary>
    /// Base class to make Authenticated calls to the WebAPI
    /// </summary>
    public abstract class AuthenticatedClientBase : IDisposable
    {
        #region constructor
        /// <summary>
        /// Constructor to initialize the base address and the token
        /// </summary>
        /// <param name="baseAddress">base address</param>
        /// <param name="bearerToken">bearer token</param>
        public AuthenticatedClientBase(string baseAddress, string bearerToken)
        {
            // perform validation
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(baseAddress);
            }

            if (string.IsNullOrEmpty(bearerToken))
            {
                throw new ArgumentNullException(bearerToken);
            }

            // assign values to member variables
            _baseAddress = baseAddress;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthHeader, bearerToken);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                }

                disposedValue = true;
                Client = null;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

        #region protected
        // HttpClient to make the calls
        protected HttpClient Client { get; private set; } = new HttpClient();
        // base address
        protected string _baseAddress;
        #endregion

        #region const
        private const string AuthHeader = "Bearer";
        #endregion
    }
}
