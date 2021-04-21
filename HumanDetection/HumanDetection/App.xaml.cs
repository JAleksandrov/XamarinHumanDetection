using System;
using System.Collections.Generic;
using System.Net.Http;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HumanDetection.UserFunctions;


namespace HumanDetection
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string username = Preferences.Get("username", "");
            string password = Preferences.Get("password", "");
            bool isLoggedIn = Preferences.Get("isLoggedIn", false);


            if (username != "" && password != "" && isLoggedIn) // Check if user logged in and contains username and password
            {

                MainPage = new MainPage(); // Open MainPage


            }
            else
            {
                MainPage = new LoginPage(); // Open Login
            }


            


          
        }

        protected override void OnStart()
        {

           

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            { // When token refresh update to API.
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");

                string username = Preferences.Get("username", "");

                Preferences.Set("notification_token", p.Token);

                var pairs = new List<KeyValuePair<string, string>>
                            {
                            new KeyValuePair<string, string>("username", username),
                            new KeyValuePair<string, string>("operating_system", GetOperatingSystem()),
                            new KeyValuePair<string, string>("token", p.Token)
                            };
                var content = new FormUrlEncodedContent(pairs);
                var client = new HttpClient { BaseAddress = new Uri("https://uni.aleksandrov.app") };
                var response = client.PostAsync("api/mobile/insert_token.php", content).Result;
                Console.WriteLine(response);
            };


        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
