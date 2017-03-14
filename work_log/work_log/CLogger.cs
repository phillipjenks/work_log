using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace work_log {

    enum WorkLogState {
        STATE_IDLE,
        STATE_TIMING,
        STATE_FINISHED,

        STATE_COUNT
    };

    class CLogger {

        private static String k_TimeFormat = "g";
        private static CultureInfo k_Culture = CultureInfo.CreateSpecificCulture("en-us");

        private WorkLogState m_state = WorkLogState.STATE_IDLE;
        public WorkLogState State {
            get { return m_state; }
        }

        private DateTime m_startTime = new DateTime();
        public String StartTime {
            get {
                return m_state == WorkLogState.STATE_TIMING || m_state == WorkLogState.STATE_FINISHED ? m_startTime.ToString(k_TimeFormat, k_Culture) : "N/A";
            }
        }

        private DateTime m_endTime = new DateTime();
        public String EndTime {
            get {
                return m_state == WorkLogState.STATE_FINISHED ? m_endTime.ToString(k_TimeFormat, k_Culture) : "N/A";
            }
        }

        private String m_logMsg;
        public String LogMessage {
            get {
                return m_logMsg;
            }
        }

        private CSaveState m_saveState = new CSaveState();
        private CLogFile m_logFile = new CLogFile();

        public CLogger() {
        }

        public void Init() {
            if(m_saveState.HasSaveState()) {
                m_startTime = m_saveState.LoadStartTime();
                m_state = WorkLogState.STATE_TIMING;
            }
        }

        public void AdvanceState() {
            switch (m_state) {
                case WorkLogState.STATE_IDLE:
                case WorkLogState.STATE_FINISHED:
                    m_state = WorkLogState.STATE_TIMING;
                    StartTiming();
                    break;
                case WorkLogState.STATE_TIMING:
                    m_state = WorkLogState.STATE_FINISHED;
                    StopTiming();
                    break;
            }
        }

        public void SetMessage(String msg) {
            m_logMsg = msg;
        }

        private void StartTiming() {
            m_startTime = DateTime.Now;
            m_saveState.SaveStartTime(m_startTime);
        }

        private void StopTiming() {
            m_endTime = DateTime.Now;
            m_logFile.SaveLog(StartTime, EndTime, m_logMsg);
            m_saveState.CleanupSaveState();
        }
    }
}
