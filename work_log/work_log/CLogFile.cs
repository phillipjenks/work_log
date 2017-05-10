using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace work_log {
    public class CLogFile {

        public static int k_interval = 6;
        private static String k_fileName = "work_log_raw.csv";
        private static String k_rawTimeFormat = "g";

        public void SaveLog(String startTime, String endTime, String msg) {
            StreamWriter fout = new StreamWriter(k_fileName, true);
            fout.WriteLine("\"" + startTime + "\",\"" + endTime + "\",\"" + msg + "\"");
            fout.Close();
        }

        private static CultureInfo k_Culture = CultureInfo.CreateSpecificCulture("en-us");
        private static String k_dayFormat = "MM/dd/yyyy";

        public void SaveLog(DateTime startTime, DateTime endTime, String msg) {
            String line = startTime.ToString(k_rawTimeFormat, k_Culture) + "," + endTime.ToString(k_rawTimeFormat, k_Culture) + ",";
                
            line += startTime.ToString(k_dayFormat, k_Culture) + ",";

            line += GenTimeString(startTime);
            line += ",";

            line += GenTimeString(endTime);
            line += ",,,\"" + msg + "\"";

            StreamWriter fout = new StreamWriter(k_fileName, true);
            fout.WriteLine(line);
            fout.Close();
        }

        public string GenTimeString(DateTime time) {
            return time.AddMinutes(GetRemainderMinutes(time.Minute)).ToString("h:mm tt", k_Culture);
        }

        public int RoundByValue(int val, int rnd) {
            int rem = val % rnd;

            if(rem > rnd / 2) {
                val += rnd - rem;
            } else {
                val -= rem;
            }

            return val;
        }

        public int GetRemainderMinutes(int mins) {
            return RoundByValue(mins, k_interval) - mins;
        }
    }
}
