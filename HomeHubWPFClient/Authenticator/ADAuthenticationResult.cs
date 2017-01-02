/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
namespace HomeHubAuthenticator
{
    /// <summary>
    /// Result for authentication
    /// </summary>
    enum ADAuthenticationResult
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
