/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;
using System.Text;
using Xamarin.Forms;

using HomeHubApp.Authenticator;
using HomeHubClient;

namespace HomeHubApp
{
    /// <summary>
    /// Code behind for MainPage
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        #region ux methods
        /// <summary>
        /// Called when user clicks Sign In/Sign Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            // if user has already signed-in, sign him/her out
            if (_authenticator.IsSignedIn)
            {
                SignOut();
                return;
            }

            try
            {
                // attempt the sign in
                await _authenticator.SignIn();
                // if successful, change the button text
                btnSignIn.Text = "Sign Out";
            }
            catch (ADAuthenticatorException ex)
            {
                // show any exception
                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Called when user clicks porch light
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPorchLight_Click(object sender, EventArgs e)
        {
            // create client using fake token
            using (Light light = new Light(_homeHubBaseAddress, "FakeToken"))
            {
                try
                {
                    // check if the light is on/off
                    bool isOn = await light.IsPorchLightOn();
                    await DisplayAlert("Result", isOn ? "On" : "Off", "OK");
                }
                catch (Exception ex)
                {
                    // show any exception
                    await DisplayAlert("Result", ex.Message, "OK");
                }
            }
        }

        /// <summary>
        /// Called when user wants to see the list of lights
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnListLights_Click(object sender, EventArgs e)
        {
            // retrieve access token from the authenticator
            string accessToken;
            try
            {
                accessToken = await _authenticator.GetToken();
            }
            catch (Exception ex)
            {
                // show any exception
                await DisplayAlert("Result", ex.Message, "OK");
                return;
            }

            // create client using base address and access token
            using (Light light = new Light(_homeHubBaseAddress, accessToken))
            {
                try
                {
                    // get the list of light ids and display it
                    var list = await light.GetListOfLights();
                    StringBuilder sb = new StringBuilder();

                    foreach (string lightId in list)
                    {
                        sb.Append(lightId);
                        sb.Append("\r\n");
                    }

                    await DisplayAlert("Result", sb.ToString(), "OK");
                }
                catch (Exception ex)
                {
                    // catch any exceptio
                    await DisplayAlert("Result", ex.Message, "OK");
                }
            }
        }

        /// <summary>
        /// Called to access lower privileged lights controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnLightOn_Click(object sender, EventArgs e)
        {
            // retrieve access token from the authenticator
            string accessToken;
            try
            {
                accessToken = await _authenticator.GetToken();
            }
            catch (ADAuthenticatorException ex)
            {
                await DisplayAlert("Result", ex.Message, "OK");
                return;
            }

            // create client using base address and access token
            using (Light light = new Light(_homeHubBaseAddress, accessToken))
            {
                try
                {
                    // turn the light on
                    await light.SwitchLight("123", true);
                    await DisplayAlert("Result", "Success", "OK");
                }
                catch (Exception ex)
                {
                    // show any exceptions
                    await DisplayAlert("Result", ex.Message, "OK");
                }
            }
        }

        /// <summary>
        /// Get secret Keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSecretKeys_Click(object sender, EventArgs e)
        {
            // retrieve access token from the authenticator
            string accessToken;
            try
            {
                accessToken = await _authenticator.GetToken();
            }
            catch (ADAuthenticatorException ex)
            {
                await DisplayAlert("Result", ex.Message, "OK");
                return;
            }

            // create client fro Vault using base address and access token
            using (Vault vault = new Vault(_homeHubBaseAddress, accessToken))
            {
                try
                {
                    // get the list of keys and display it
                    var list = await vault.GetSecretKeys();
                    StringBuilder sb = new StringBuilder();

                    foreach (string key in list)
                    {
                        sb.Append(key);
                        sb.Append("\r\n");
                    }

                    await DisplayAlert("Result", sb.ToString(), "OK");
                }
                catch (Exception ex)
                {
                    // catch any exception
                    await DisplayAlert("Result", ex.Message, "OK");
                }
            }
        }

        #endregion

        #region worker methods
        /// <summary>
        /// perform signout
        /// </summary>
        private void SignOut()
        {
            // ask authenticator to sign out
            _authenticator.SignOut();
            // change the button text
            btnSignIn.Text = "Sign In";
        }
        #endregion

        #region members
        // get base address url
        private static string _homeHubBaseAddress = Constants.HomeHubBaseAddress;
        // authenticator
        private IADAuthenticator _authenticator = DependencyService.Get<IADAuthenticator>();
        #endregion
    }
}
