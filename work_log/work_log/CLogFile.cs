using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace work_log {
    class CLogFile {

        static String k_fileName = "work_log_raw.csv";

        public void SaveLog(String startTime, String endTime, String msg) {
            StreamWriter fout = new StreamWriter(k_fileName, true);
            fout.WriteLine("\"" + startTime + "\",\"" + endTime + "\",\"" + msg + "\"");
            fout.Close();
        }
    }
}
