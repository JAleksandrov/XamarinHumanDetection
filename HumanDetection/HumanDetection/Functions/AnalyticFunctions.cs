using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HumanDetection
{
    public class AnalyticFunctions
    {

        public static Color GetCalendarColour(int Humans_Detected)
        {
            Color Assigned_Colour = Color.Green;
            // By Default Colour is Green
            // Will stay green >5

            if (Humans_Detected >= 5 && Humans_Detected < 15) // Between 5 and 15
            { // Turn colour Orange
                Assigned_Colour = Color.Orange;
            }
            else if (Humans_Detected >= 15) // Over 15 colour red
            {
                Assigned_Colour = Color.Red;
            }

            return Assigned_Colour;
        }

        public static string GetTodayDate()
        { // Gets today date in dd/mm/yyyy format.
            return DateTime.Today.ToString("dd/MM/yyyy");
        }

        private static string FormatTwoDigits(int value)
        {   // Converts number 1 -> 01
            return value.ToString("00");
        }


        public static int GetHumanDetectionBasedOnDay(int Day)
        {   // Gets this month amount of analysed images based on this months day.
            string date_format = FormatTwoDigits(Day) + "/" + FormatTwoDigits(DateTime.Now.Month) + "/" + DateTime.Now.Year;
            return Preferences.Get("Analysed_Images_" + date_format, 0);
        }


        public static int GetThisMonth()
        {
            // Provides this months human detections


            int Days_This_Month = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            // Gets the amount of this month days.

            int Humans_Detected = 0;
            for (int Day = 1; Day < Days_This_Month; Day++)
            {   // forloop this day by day for whole month
                Humans_Detected += GetHumanDetectionBasedOnDay(Day);
            }

            return Humans_Detected;
        }

        public static int GetThisWeek()
        {
            var StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            //Get this week starting day

            var EndDate = DateTime.Today; // Date of today

            int Humans_Detected = 0;
            int numberOfDays = (EndDate - StartDate).Days; // finds the number of days for the period.
            for (int Day = 0; Day <= numberOfDays; Day++) // Forloop the amount of days
            {
                Humans_Detected += GetHumanDetectionBasedOnDay(Day); // Adds to human detected the value
                StartDate = StartDate.AddDays(1);          // Adds the day to start date
            }

            return Humans_Detected;
        }

    }
}
