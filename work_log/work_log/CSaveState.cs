using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace work_log {
    class CSaveState {

        private static String k_saveFile = ".work_log_start_time.dat";

        private String FilePath {
            get {
                return k_saveFile;
            }
        }

        public bool HasSaveState() {
            return File.Exists(FilePath);
        }

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

        public void SaveStartTime(DateTime time) {
            CleanupSaveState();

            FileStream fout = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            fout.Write(BitConverter.GetBytes(time.ToBinary()), 0, sizeof(long));
            fout.Close();
        }

        public void CleanupSaveState() {
            if(HasSaveState()) {
                File.Delete(FilePath);
            }
        }
    }
}
