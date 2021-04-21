using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using static HumanDetection.JsonClasss;
using static HumanDetection.UserFunctions;
using static HumanDetection.AnalyticFunctions;
using Plugin.FirebasePushNotification;

namespace HumanDetection
{
    public partial class AnalyticsPage : ContentPage
    {
        public AnalyticsPage()
        {
            InitializeComponent();

            Sound_Button.Text = GetSoundText(); // Initilize sound button text
            Display();  // Set from memory text and calendar colours
            FetchAnalytics(); // Get new data from API.
            
        }


   


        private void setCalendarColours()
        {   
            List<XamForms.Controls.SpecialDate> DetectionDates = new List<XamForms.Controls.SpecialDate>(); //Initilize varaible for calendar colour

            int Days_This_Month = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
         
            for (int Day = 1; Day <= Days_This_Month; Day++) // Forloop this month days.
            {
                int Humans_Detected = GetHumanDetectionBasedOnDay(Day); // Get human detection based on day

                if (Humans_Detected != 0)
                {   // Add to list the special dates.            
                    DetectionDates.Add(new XamForms.Controls.SpecialDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, Day)) { BorderColor = GetCalendarColour(Humans_Detected), BorderWidth = 5, Selectable = true });
                }
            }

            Calendar.SpecialDates = DetectionDates; // Update calendar with new colour dates.
        }

       
        private void Display()
        {
            // Update display with latest variables from memory
            string TodayDate = GetTodayDate();
                      
            
            Today_Date_Text.Text = Preferences.Get("Analysed_Images_" + TodayDate, 0).ToString();
            Week_Date_Text.Text = GetThisWeek().ToString();
            Month_Date_Text.Text = GetThisMonth().ToString();
            setCalendarColours();

        }

        private async void FetchAnalytics()
        {
            string username = Preferences.Get("username", "");
            string password = Preferences.Get("password", "");         
            string Analytics_Fetched_Response = await GetAnalytics(username, password);
            // Get latest analytic data

            if (Analytics_Fetched_Response != null)
            {
                var Analytics_Fetched = JsonConvert.DeserializeObject<List<Analytics>>(Analytics_Fetched_Response);

                for (int i = 0; i < Analytics_Fetched.Count; i++)
                {

                    int Analysed_Images = Analytics_Fetched[i].Analysed_Images;
                    string Analysed_Date = Analytics_Fetched[i].Analysed_Date;
                    Preferences.Set("Analysed_Images_" + Analysed_Date, Analysed_Images);
                    // Store to preferences.
                }
            }
           // Update to latest data
            Display();
        }


        void Back_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //back button clicked 
            Application.Current.MainPage = new MainPage(); // return to home page
        }

        void Sound_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //Sound button clicked
            bool Current_Sound_Option = Preferences.Get("Sound_Option", false);
            Preferences.Set("Sound_Option", !Current_Sound_Option);
            Sound_Button.Text = GetSoundText();
        }


        async void Logout_Button_Clicked(System.Object sender, System.EventArgs e)
        {   // Logout button clicked

            bool Logout = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No"); // Ask user if they want to logout

            if (Logout) // If true logout
            {
                ForceLogout();
            }
        }


        private void ForceLogout()
        {
            Preferences.Set("isLoggedIn", false); // Set preference isLoggedIn to False
            CrossFirebasePushNotification.Current.UnregisterForPushNotifications(); // Unregister notifications
            Application.Current.MainPage = new NavigationPage(new LoginPage()); // Move Page to LoginPage
        }

    }
}
