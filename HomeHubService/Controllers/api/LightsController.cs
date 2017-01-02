/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

using HomeHubService.Utils;
using HomeHubService.Attributes;
using HomeHub.Model;

namespace HomeHubService.Controllers
{
    /// <summary>
    /// This light controller demonstrates various levels of authorization
    /// </summary>
    public class LightsController : ApiController
    {
        /// <summary>
        /// Anonymous access to see if porch light is on
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous] // allow anyone
        [ActionName("IsPorchLightOn")]
        [HttpGet]
        public bool IsPorchLightOn()
        {
            // return on/off based on what second itis called
            return ((DateTime.Now.Second % 10) > 5);
        }

        /// <summary>
        /// Any authorized user can get the list of lights
        /// No  group based authorization required
        /// </summary>
        /// <returns></returns>
        [Authorize] // allow any authorized user
        [ActionName("List")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // returns light ids
            return new string[] { "light 1", "light 2" };
        }

        // PUT api
        /// <summary>
        /// Access allowed only if user belongs to Group corresponding to Level 1
        /// </summary>
        /// <param name="lightSwitch">Model</param>
        [ADGroupAuthorizeLevel1] // allow only those user who belong to group level 1
        [ActionName("switch")]
        [HttpPut]
        public void Put([FromBody]LightSwitchModel lightSwitch)
        {
            // just write the model values
            Debug.WriteLine($"Light Switch {lightSwitch.Id} is {lightSwitch.IsOn}");
        }
    }
}
