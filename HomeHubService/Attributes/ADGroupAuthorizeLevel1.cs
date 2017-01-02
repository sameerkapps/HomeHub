/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Web.Http;
using System.Web.Http.Controllers;

using HomeHubService.Utils;

namespace HomeHubService.Attributes
{
    /// <summary>
    /// Authorization attribute for group level 1
    /// </summary>
    public class ADGroupAuthorizeLevel1 : AuthorizeAttribute
    {
        /// <summary>
        /// Checks, if the current user is as per authorization Level 1
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return GroupHelper.IsLevel1;
        }
    }
}