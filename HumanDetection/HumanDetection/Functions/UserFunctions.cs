using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HumanDetection
{
    public class UserFunctions
    {

        public static string GetOperatingSystem()
        {
            if (Device.RuntimePlatform == Device.iOS) // Check if device is iOS
            {
                return "iOS"; // Return iOS
            }

        return "Android"; // Default value android
        }

        public static async Task<string> GetAnalytics(string username, string password)
        {
            //API to get analytic statistic.

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://uni.aleksandrov.app/api/mobile/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();
                response = await client.GetAsync("get_analytics.php?username=" + username + "&password=" + password).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {   // If something is found
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {   //Nothing found then returns null
                    return null;
                }
            }

        }


        public static async Task<string> GetImages(string username, string password)
        {
            // Get the images from API

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://uni.aleksandrov.app/api/mobile/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();
                response = await client.GetAsync("get_images.php?username=" + username + "&password=" + password).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    // If something is found
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {   // If nothing is found
                    return null;
                }
            }

        }

        public static async Task<System.Net.HttpStatusCode> Authorization(string username, string password)
        {
            // Authorization without notification token

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://uni.aleksandrov.app/api/mobile/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();
                response = await client.GetAsync("login_api.php?username=" + username + "&password=" + password).ConfigureAwait(false);
                return response.StatusCode;
            }
        }

        public static async Task<System.Net.HttpStatusCode> Authorization(string username, string password, string token)
        {
            //Authorization with notification token

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://uni.aleksandrov.app/api/mobile/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();
                response = await client.GetAsync("login_api.php?username=" + username + "&password=" + password + "&token=" + token).ConfigureAwait(false);
                return response.StatusCode;
            }
        }




        public static string GetSoundText()
        {
            bool Current_Sound_Option = Preferences.Get("Sound_Option", false); // By default is false
            string Sound_Text = "Sound "; // template

            if (Current_Sound_Option)
            {
                Sound_Text += "ON"; // Outcome "Sound ON"
            }
            else
            {
                Sound_Text += "OFF"; // Outcome "Sound OFF"
            }

            return Sound_Text; //Return variable
        }







    }

}
