/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HomeHubClient.Utils;

namespace HomeHubClient
{
    /// <summary>
    /// Client for Vault API
    /// </summary>
    public class Vault : AuthenticatedClientBase
    {
        #region constructor
        /// <summary>
        /// Constructor to initialize the base address and the token
        /// </summary>
        /// <param name="baseAddress">base address</param>
        /// <param name="bearerToken">bearer token</param>
        public Vault(string baseAddress, string bearerToken)
            :base(baseAddress, bearerToken)
        {

        }
        #endregion

        /// <summary>
        /// Retreives secret keys from the Vault controller
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetSecretKeys()
        {
            // call the API
            HttpResponseMessage response = await Client.GetAsync(_baseAddress + "/api/vault/GetSecretKeys/");

            // if the call is successful
            // read the value and convert to list
            if (response.StatusCode.IsSuccessful())
            {
                var result = await response.Content.ReadAsStringAsync();

                var secretKeys = JsonConvert.DeserializeObject<List<string>>(result);

                return secretKeys;
            }

            // if the call fails, throw exception
            throw new Exception(response.ReasonPhrase);
        }
    }
}
