using System;
namespace HumanDetection
{
    public class JsonClasss
    {
        // Store JSON response from API.

        public class Images_Analysed
        {
            
            public int Analysed_ID { get; set; }
            public string File_Name { get; set; }
            public string Confidence { get; set; }
            public string Analysed_Date { get; set; }

        }

        public class Analytics
        {

            public int Analysed_Images { get; set; }
            public string Analysed_Date { get; set; }


        }


    }
}
