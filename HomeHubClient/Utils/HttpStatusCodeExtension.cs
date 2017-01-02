/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Net;

namespace HomeHubClient.Utils
{
    /// <summary>
    /// Extension for HttpStatusCode
    /// </summary>
    public static class HttpStatusCodeExtension
    {
        /// <summary>
        /// Determines, if the status is successful or not
        /// Useful if you do not have access to HttpResponse object
        /// </summary>
        /// <param name="statusCode">status code</param>
        /// <returns></returns>
        public static bool IsSuccessful(this HttpStatusCode statusCode)
        {
            return statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.MultipleChoices;
        }
    }
}
