using System;
using Xamarin.Essentials;

namespace HumanDetection
{
    public class ImageFunctions
    {

        public static string GetLatestImage()
        {
            int Images_Fetched_Count = Preferences.Get("Images_Fetched_Count", 0); // The amount of images

            int Analysed_ID = Preferences.Get("Analysed_ID_" + (Images_Fetched_Count - 1), 0); // Based on Image count it will select the analysed id

            if (Analysed_ID != 0) // if analysed is found
            {
                return Preferences.Get("File_Name_" + Analysed_ID, ""); // provide file name based on analysed id
            }

            return null; // return nothing
        }

        public static string GetBackImage(string Current_Showing_Image)
        {
            int Images_Fetched_Count = Preferences.Get("Images_Fetched_Count", 0); // Get Image count


            for (int i = 0; i < Images_Fetched_Count; i++) // for loop
            {
                int Analysed_ID = Preferences.Get("Analysed_ID_" + i, 0); // get analysed id based on id
                string File_Name = Preferences.Get("File_Name_" + Analysed_ID, ""); // get file name based on analysed id

                if (Current_Showing_Image == File_Name) // if something is found
                {
                    Console.WriteLine(File_Name + " is Iteration ID " + i);
                    if (i != 0)
                    {
                        return GetFile_Name(i - 1); // get back image
                    }
                    else
                    {
                        return GetLatestImage(); // get lastest image
                    }
                                   
                }

            }


            return GetFile_Name(0); // get first image
        }

        public static string GetNextImage(string Current_Showing_Image)
        { 
            int Images_Fetched_Count = Preferences.Get("Images_Fetched_Count", 0);
          

            for (int i = 0; i < Images_Fetched_Count; i++) // forloop image count
            {
                int Analysed_ID = Preferences.Get("Analysed_ID_" + i, 0); // Find current image ID
                string File_Name = Preferences.Get("File_Name_" + Analysed_ID, ""); // used for validation

                if (Current_Showing_Image == File_Name) // when found same as current image
                {
                    Console.WriteLine(File_Name + " is Iteration ID " + i);
                   
                    return GetFile_Name(i+1); // Current image Iteration ID + 1 which is next image 
                   
                    
                }

            }


            return GetFile_Name(0); // return first image
        }


        private static string GetFile_Name(int Iteration_ID)
        {
            // Get file name based on iteration id

            Console.WriteLine("Searching for Iteration ID " + Iteration_ID);
            int Current_Analysed_ID = Preferences.Get("Analysed_ID_" + Iteration_ID, 0); // get analysed ID based on iteration id

            if (Current_Analysed_ID != 0) // if found something
            {
                string File_Name = Preferences.Get("File_Name_" + Current_Analysed_ID, ""); // get file name

                if (File_Name != "") // check for filename validation
                {
                    return File_Name; //return file name
                }

            }

            Current_Analysed_ID = Preferences.Get("Analysed_ID_" + 0, 0); // get first analysed id
            
            return Preferences.Get("File_Name_" + Current_Analysed_ID, ""); // return first file name
        }




        public static string[] GetImageDetails(string Current_Showing_Image)
        {
            string[] Details = new string[2]; // two row array for Confidence/Analysed ID

            int Images_Fetched_Count = Preferences.Get("Images_Fetched_Count", 0); // all images


            for (int i = 0; i < Images_Fetched_Count; i++) // forloop through all images
            {
                int Analysed_ID = Preferences.Get("Analysed_ID_" + i, 0); // based on iteration id
                string File_Name = Preferences.Get("File_Name_" + Analysed_ID, ""); // find file name

                if (Current_Showing_Image == File_Name) // check if current image is matching found file name
                {
                    Details[0] = Preferences.Get("Confidence_" + Analysed_ID, ""); // Confidence 
                    Details[1] = Preferences.Get("Analysed_Date_" + Analysed_ID, ""); // Analysed iD 
                }
            }

            return Details; // find details
        }


 


    }
}
