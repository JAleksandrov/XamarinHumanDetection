using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using static HumanDetection.JsonClasss;
using static HumanDetection.ImageFunctions;
using static HumanDetection.UserFunctions;
using Plugin.FirebasePushNotification;

namespace HumanDetection
{
    public partial class MainPage : ContentPage
    {
        
        string Current_Showing_Image = "";
        string Image_Directory = "https://uni.aleksandrov.app/human_detection/analyzed/"; // Directory to read images from

        public MainPage()
        {
            InitializeComponent();

            Sound_Button.Text = GetSoundText(); // initilize sound button text

            FetchOnlineImages(); // Get data from api for latest image
   
        }



        void Back_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Back button clicked
            Current_Showing_Image = GetBackImage(Current_Showing_Image);
            Current_Image.Source = Image_Directory + Current_Showing_Image;
            UpdateImageText(Current_Showing_Image);
        }


        void Next_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Next button clicked
            Current_Showing_Image = GetNextImage(Current_Showing_Image);
            Current_Image.Source = Image_Directory + Current_Showing_Image;
            UpdateImageText(Current_Showing_Image);

        }


        void Analytics_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Analytic Button clicked
            Application.Current.MainPage = new AnalyticsPage();
        }


        void Sound_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Sound button clicked
            bool Current_Sound_Option = Preferences.Get("Sound_Option", false); // Get current calue
            Preferences.Set("Sound_Option", !Current_Sound_Option); // Invert value and save
            Sound_Button.Text = GetSoundText(); // Update text

            
        }


        async void Logout_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // logout button clicked
            bool Logout = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No"); // ask user for confirmation

            if (Logout) // if confirmation is true
            {
                ForceLogout(); // log user out
            }
           
        }



        private async void CheckLoginDetails(string username, string password)
        {
            if (username != "" && password != "")
            {
                var Authentication_Response = await Authorization(username, password);

                if (Authentication_Response != System.Net.HttpStatusCode.OK)
                { // if status code is not OK force logout
                    ForceLogout();
                }

            }
            else
            {   // If no username or password is supplied
                ForceLogout();
            }
        }






       


        private void SetLatestImage()
        {
            Current_Showing_Image = GetLatestImage();
            // Get latest image
            if (Current_Showing_Image != null)
            {
                UpdateImageText(Current_Showing_Image); // Update Image text
                Current_Image.Source = Image_Directory + Current_Showing_Image; // Set Image source
                if (!Image_Controls.IsVisible)
                    Image_Controls.IsVisible = true;

                if (!Image_Description.IsVisible)
                    Image_Description.IsVisible = true;

                if (!Current_Image.IsVisible)
                    Current_Image.IsVisible = true;
            }
            else
            {
                NotFoundImage(); // display not found image
            }
            
        }

        private void NotFoundImage()
        {   // Display not found text
            Current_Image.IsVisible = false;
            Not_Detected_Text.IsVisible = true;

        }


        private async void FetchOnlineImages()
        {
            string username = Preferences.Get("username", "");
            string password = Preferences.Get("password", "");

            CheckLoginDetails(username, password); // check user credentials

            string Images_Fetched_Response = await GetImages(username, password); // get images

            if (Images_Fetched_Response != null) // if images found
            {
                var Images_Fetched = JsonConvert.DeserializeObject<List<Images_Analysed>>(Images_Fetched_Response); // store into object

                Preferences.Set("Images_Fetched_Count", Images_Fetched.Count); // store images found

                for (int i = 0; i < Images_Fetched.Count; i++) // loop through images
                {

                    
                    int Analysed_ID = Images_Fetched[i].Analysed_ID;
                    string File_Name = Images_Fetched[i].File_Name;
                    string Confidence = Images_Fetched[i].Confidence;
                    string Analysed_Date = Images_Fetched[i].Analysed_Date;

                    int Current_Analysed_ID = Preferences.Get("Analysed_ID_" + i, 0);

                    if (Current_Analysed_ID != Analysed_ID)
                    {
                        // Store image data to preference
                        Preferences.Set("Analysed_ID_" + i, Analysed_ID);
                        Preferences.Set("File_Name_" + Analysed_ID, File_Name);
                        Preferences.Set("Confidence_" + Analysed_ID, Confidence);
                        Preferences.Set("Analysed_Date_" + Analysed_ID, Analysed_Date);
                    }

                }
            }

            Console.WriteLine(Images_Fetched_Response);
            SetLatestImage(); // update image

        }


        private void UpdateImageText(string Current_Showing_Image)
        {
            string[] Details = GetImageDetails(Current_Showing_Image); //Get image details based on variable

            Confidence_Text.Text = Details[0]; // Set confidence
            Date_Text.Text = Details[1]; // Set date
        }


        private void ForceLogout()
        {
            Preferences.Set("isLoggedIn", false); // Set preference isLoggedIn to False
            CrossFirebasePushNotification.Current.UnregisterForPushNotifications(); // Unregister notifications
            Application.Current.MainPage = new NavigationPage(new LoginPage()); // Move Page to LoginPage
        }


      
    }
}
