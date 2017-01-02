using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using HomeHubApp.Authenticator;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Android.Content;

namespace HomeHubApp.Droid
{
    [Activity(Label = "HomeHubApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            DependencyService.Register<IADAuthenticator, ADAuthenticator>();

            var iadAuth = DependencyService.Get<IADAuthenticator>();
            iadAuth.Configure(Constants.AADInstance,
                              Constants.Tenant,
                              Constants.ClientId,
                              Constants.RedirectUri,
                              Constants.HomeHubResourceId);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode,
                data);
        }
    }
}

