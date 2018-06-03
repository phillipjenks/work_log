using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace work_log {

    // Class to handle writing sessions to a log file
    public class CLogFile {

        // Sessions restrained to 6 minute intervals
        public static int k_interval = 6;

        // The file to save the log in
        private static String k_fileName = "work_log_raw.csv";

        // Formatting for the output time stamps 
        private static String k_rawTimeFormat = "g";
        private static CultureInfo k_Culture = CultureInfo.CreateSpecificCulture("en-us");
        private static String k_dayFormat = "MM/dd/yyyy";

        // Given a start time, end time, msg, and optionally a category, write this current work session to the work log
        public void SaveLog(DateTime startTime, DateTime endTime, String msg, String category = "UNKNOWN") {
            String line = startTime.ToString(k_rawTimeFormat, k_Culture) + "," + endTime.ToString(k_rawTimeFormat, k_Culture) + ",";
                
            line += startTime.ToString(k_dayFormat, k_Culture) + ",";

            line += GenTimeString(startTime);
            line += ",";

            line += GenTimeString(endTime);
            line += ",,\"" + category + "\",\"" + msg + "\"";

            StreamWriter fout = new StreamWriter(k_fileName, true);
            fout.WriteLine(line);
            fout.Close();
        }

        // Get a string with a time rounded off to the nearest interval
        public string GenTimeString(DateTime time) {
            return time.AddMinutes(GetRemainderMinutes(time.Minute)).ToString("h:mm tt", k_Culture);
        }

        // Given a value and an interval, round the value to the nearest multiple of the interval
        public int RoundByValue(int val, int rnd) {
            int rem = val % rnd;

            if(rem > rnd / 2) {
                val += rnd - rem;
            } else {
                val -= rem;
            }

            return val;
        }

        // Utility function to get the difference between the given time and the rounded time
        public int GetRemainderMinutes(int mins) {
            return RoundByValue(mins, k_interval) - mins;
        }
    }
}
