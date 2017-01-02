/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
namespace HomeHubApp.Authenticator
{
    /// <summary>
    /// Result for authentication
    /// </summary>
    public enum ADAuthenticationResult
    {
        /// <summary>
        /// Successfully authenticated
        /// </summary>
        Success,
        /// <summary>
        /// User cancelled sign-in
        /// </summary>
        SignInCancelled,
        /// <summary>
        /// Sign-in required to obtain token
        /// </summary>
        SignInRequired,
        /// <summary>
        /// Other AdalException was thrown
        /// </summary>
        AdalError
    }
}
