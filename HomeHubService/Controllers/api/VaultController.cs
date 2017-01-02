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

namespace HomeHubService.Controllers
{
    /// <summary>
    /// This controller demostrates authorization for Secret level
    /// </summary>
    public class VaultController : ApiController
    {
        /// <summary>
        /// Authorization allowed only if the user belongs to Secret Level Group
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetSecretKeys")]
        [ADGroupAuthorizeSecretLevel]
        public IEnumerable<string> Get()
        {
            return new string[] { "key1", "key2" };
        }
    }
}
