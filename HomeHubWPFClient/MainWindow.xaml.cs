/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Windows;

using HomeHubAuthenticator;
using HomeHubClient;
using HomeHubWPFClient.Authenticator;

namespace HomeHubWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Did not have time to do MVVM here
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region ux methods
        /// <summary>
        /// Called when user clicks Sign In/Sign Out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
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
                btnSignIn.Content = "Sign Out";
            }
            catch (ADAuthenticatorException ex)
            {
                // show any exception
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Called when user clicks porch light
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPorchLight_Click(object sender, RoutedEventArgs e)
        {
            // create client using fake token
            using (Light light = new Light(_homeHubBaseAddress,"FakeToken"))
            {
                try
                {
                    // check if the light is on/off
                    bool isOn = await light.IsPorchLightOn();
                    MessageBox.Show(this, isOn ? "On" : "Off");
                }
                catch (Exception ex)
                {
                    // show any exception
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Called when user wants to see the list of lights
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnListLights_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show(this, ex.Message);
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

                    MessageBox.Show(this, sb.ToString());
                }
                catch (Exception ex)
                {
                    // catch any exceptio
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Called to access lower privileged lights controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnLightOn_Click(object sender, RoutedEventArgs e)
        {
            // retrieve access token from the authenticator
            string accessToken;
            try
            {
                accessToken = await _authenticator.GetToken();
            }
            catch (ADAuthenticatorException ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }

            // create client using base address and access token
            using (Light light = new Light(_homeHubBaseAddress, accessToken))
            {
                try
                {
                    // turn the light on
                    await light.SwitchLight("123", true);
                    MessageBox.Show(this, "Success");
                }
                catch(Exception ex)
                {
                    // show any exceptions
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Get secret Keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSecretKeys_Click(object sender, RoutedEventArgs e)
        {
            // retrieve access token from the authenticator
            string accessToken;
            try
            {
                accessToken = await _authenticator.GetToken();
            }
            catch (ADAuthenticatorException ex)
            {
                MessageBox.Show(this, ex.Message);
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

                    MessageBox.Show(this, sb.ToString());
                }
                catch (Exception ex)
                {
                    // catch any exception
                    MessageBox.Show(this, ex.Message);
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
            btnSignIn.Content = "Sign In";
        }
        #endregion

        #region members
        // get base address url
        private static string _homeHubBaseAddress = ConfigurationManager.AppSettings["homehub:HomeHubBaseAddress"];
        // authenticator
        private ADAuthenticator _authenticator = new ADAuthenticator();
        #endregion
    }
}
