using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHubApp
{
    public static class Constants
    {
        #region AD constants
        /// <summary>
        /// Your AADInstance
        /// </summary>
        public const string AADInstance = "https://login.microsoftonline.com/{0}";
        /// <summary>
        /// Tenant from your active directory
        /// e.g.contoso.onmicrosoft.com
        /// </summary>
        public const string Tenant = "TODO";
        /// <summary>
        /// Id of the client from Active Directory where you registered
        /// the "Client" application
        /// </summary>
        public const string ClientId = "TODO";
        /// <summary>
        /// Any valid uri name e.g. http://YourService
        /// </summary>
        public const string RedirectUri = "TODO";
        /// <summary>
        /// Appid uri for the "Service"
        /// You will find it in ActiveDirectory -> Your Service -> Confiure Tab -> Single Sign-on section ->APP ID URI 
        /// e.g. https://contoso.onmicrosoft.com/YourService
        /// </summary>
        public const string HomeHubResourceId = "TODO";
        #endregion

        #region App location
        /// <summary>
        /// URI of the published service
        /// e.g. https://yourservice.azurewebsites.net/
        /// default for this solution http://localhost:20090/
        /// </summary>
        public const string HomeHubBaseAddress = "http://localhost:20090/";
        #endregion
    }
}
