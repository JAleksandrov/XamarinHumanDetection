using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using RestSharp;
using Xamarin.Essentials;
using Xamarin.Forms;
using static HumanDetection.UserFunctions;




namespace HumanDetection
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            if(Preferences.Get("RememberMe", false)) // if remember me button was pressed
            {
                // Insert data into fields from memory

                Remember_Me_Checkbox.IsChecked = true; 
                
                EntryUsername.Text = Preferences.Get("username", "");
                EntryPassword.Text = Preferences.Get("password", "");
            }


        }

     


        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Login button clicked

            string username = EntryUsername.Text;
            string password = EntryPassword.Text;


            string token = Preferences.Get("notification_token", "");


            if (username != "" && password != "")
            {
                System.Net.HttpStatusCode Authentication_Response;

                if (token != "")
                {   // If token is found it will use this function
                    Authentication_Response = await Authorization(username, password, token);
                }
                else
                {   // If no token was found it will use this function
                    Authentication_Response = await Authorization(username, password);
                }               

                if (Authentication_Response == System.Net.HttpStatusCode.OK)
                {   // If status code is OK it will login to main page
                    Console.WriteLine("OK");
                    CrossFirebasePushNotification.Current.RegisterForPushNotifications();
                    Preferences.Set("username", username);
                    Preferences.Set("password", password);
                    Preferences.Set("isLoggedIn", true);
                    Preferences.Set("RememberMe", Remember_Me_Checkbox.IsChecked);

                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else if (Authentication_Response == System.Net.HttpStatusCode.Unauthorized)
                {   // Wrong Password was entered
                    Console.WriteLine("Unauthorized");
                    await DisplayAlert("Login Failed", "Username or Password is incorrect", "OK");
                }
                else if (Authentication_Response == System.Net.HttpStatusCode.BadRequest)
                {   // API received bad parameters
                    Console.WriteLine("BadRequest");
                    await DisplayAlert("Login", "Something Went Wrong!","OK");
                }
                


            }
            else
            {   // Username or Password is empty
                await DisplayAlert("Alert", "Username or Password is empty", "OK");
            }

        }


 

       





    }
}
