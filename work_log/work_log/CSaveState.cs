using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace work_log {

    // Class to represent the current save state of the work log
    // The save state is the start time of the current session serialized as the binary version
    // of the DateTime object
    class CSaveState {

        // File to save the save state
        private static String k_saveFile = ".work_log_start_time.dat";

        // File that contains the possible categories
        private static String k_catFile = "work_log_categories.txt";

        private String FilePath {
            get {
                return k_saveFile;
            }
        }

        // Is there an existing save state?
        public bool HasSaveState() {
            return File.Exists(FilePath);
        }

        // Loads the current start time of the session. If there is none, return a default DateTime
        public DateTime LoadStartTime() {
            if(!HasSaveState()) {
                return new DateTime();
            }

            FileStream fin = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            byte[] data = new byte[sizeof(long)];
            fin.Read(data, 0, sizeof(long));
            fin.Close();

            return DateTime.FromBinary(BitConverter.ToInt64(data, 0));
        }

        // Saves the current start time of the session
        public void SaveStartTime(DateTime time) {
            CleanupSaveState();

            FileStream fout = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            fout.Write(BitConverter.GetBytes(time.ToBinary()), 0, sizeof(long));
            fout.Close();
        }

        // Removes the existing save state
        public void CleanupSaveState() {
            if(HasSaveState()) {
                File.Delete(FilePath);
            }
        }

        // Load given categories if they exist
        public void LoadCategories(HashSet<String> cats) {
            if(File.Exists(k_catFile)) {

                String[] lines = File.ReadAllLines(k_catFile);
                foreach(String line in lines) {
                    String trimLine = line.Trim(' ');
                    if (trimLine.Length > 0) {
                        cats.Add(trimLine);
                    }
                }
            }
        }
    }
}
