/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HomeHub.Model;
using HomeHubClient.Utils;

namespace HomeHubClient
{
    /// <summary>
    ///  client class to expose WebAPIs for light
    /// </summary>
    public class Light : AuthenticatedClientBase
    {
        /// <summary>
        /// Constructor to initialize the base address and the token
        /// </summary>
        /// <param name="baseAddress">base address</param>
        /// <param name="bearerToken">bearer token</param>
        public Light(string baseAddress, string bearerToken)
            :base(baseAddress, bearerToken)
        {
        }

        /// <summary>
        /// Corresponds to IsPorchLightOn API
        /// This is anonymous call. No autentication needed
        /// </summary>
        /// <returns>true if on else false</returns>
        public async Task<bool> IsPorchLightOn()
        {
            // creates local HttpClient to prevent authentication
            HttpClient anonymousClient = new HttpClient();

            // call the API
            HttpResponseMessage response = await anonymousClient.GetAsync(_baseAddress + "/api/lights/IsPorchLightOn/");

            // if the result is successful
            // convert to bool and return it
            if (response.StatusCode.IsSuccessful())
            {
                var result = await response.Content.ReadAsStringAsync();
                return bool.TrueString.Equals(result, StringComparison.OrdinalIgnoreCase);
            }

            // if the call fails throw exception with reason
            throw new Exception(response.ReasonPhrase);
        }

        /// <summary>
        /// Retrieves list of light ids from the WebAPI
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetListOfLights()
        {
            // call the API
            HttpResponseMessage response = await Client.GetAsync(_baseAddress + "/api/lights/List/");

            // if the result is successful
            // convert to list of strings and return it
            if (response.StatusCode.IsSuccessful())
            {
                var result = await response.Content.ReadAsStringAsync();
                var lights = JsonConvert.DeserializeObject<List<string>>(result);

                return lights;
            }

            // if the call fails throw exception with reason
            throw new Exception(response.ReasonPhrase);
        }

        /// <summary>
        /// Makes a call to WebAPI to turn light on/off
        /// </summary>
        /// <param name="lightId">Id of the light</param>
        /// <param name="turnOn">on/off flag</param>
        /// <returns></returns>
        public async Task SwitchLight(string lightId, bool turnOn)
        {
            // validate parameters
            if (string.IsNullOrEmpty(lightId))
            {
                throw new ArgumentNullException(nameof(lightId));
            }

            // Create the model to be transmitted
            var lightswichModel = new LightSwitchModel() { Id = lightId, IsOn = turnOn };
            // convert it to JSON
            var modelJson = JsonConvert.SerializeObject(lightswichModel);

            // add model to the reuest as content with the right content type
            HttpContent content = new StringContent(modelJson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // make that call
            HttpResponseMessage response = await Client.PutAsync(_baseAddress + "/api/lights/switch/", content);

            // if the call is not successful, throw exception
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
