using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace work_log {

    // Enum for the state of the UI
    enum WorkLogState {
        STATE_IDLE,
        STATE_TIMING,
        STATE_FINISHED,

        STATE_COUNT
    };

    // Class to handle interaction with the UI and to interface with the save state of work logger
    class CLogger {

        // Formatting utilities for print time stamps to the UI
        private static String k_TimeFormat = "g";
        private static CultureInfo k_Culture = CultureInfo.CreateSpecificCulture("en-us");

        // The current work log state
        private WorkLogState m_state = WorkLogState.STATE_IDLE;
        public WorkLogState State {
            get { return m_state; }
        }

        // Start time of the current session
        private DateTime m_startTime = new DateTime();
        public String StartTime {
            get {
                return m_state == WorkLogState.STATE_TIMING || m_state == WorkLogState.STATE_FINISHED ? m_startTime.ToString(k_TimeFormat, k_Culture) : "N/A";
            }
        }

        // End time of the current session
        private DateTime m_endTime = new DateTime();
        public String EndTime {
            get {
                return m_state == WorkLogState.STATE_FINISHED ? m_endTime.ToString(k_TimeFormat, k_Culture) : "N/A";
            }
        }

        // Message for the current session
        private String m_logMsg;
        public String LogMessage {
            get {
                return m_logMsg;
            }
        }

        // Category for the current session
        private String m_category;
        public String Category {
            get {
                return m_category;
            }
        }

        // A set containing the possible work categories
        public HashSet<String> CategorySet = new HashSet<String>();

        // The save state and log file classes
        private CSaveState m_saveState = new CSaveState();
        private CLogFile m_logFile = new CLogFile();

        public CLogger() {
        }

        // Load our save state if we have one and set our state if necessary 
        public void Init() {
            if(m_saveState.HasSaveState()) {
                m_startTime = m_saveState.LoadStartTime();
                m_state = WorkLogState.STATE_TIMING;
            }

            m_saveState.LoadCategories(CategorySet);
        }

        // Advance work log state
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

        // Sets the message for the current work log
        public void SetMessage(String msg) {
            m_logMsg = msg;
        }

        // Sets the category for the current work log
        public void SetCategory(String cat) {
            m_category = cat;
        }

        // Start time the current session and save the start time to disk
        private void StartTiming() {
            m_startTime = DateTime.Now;
            m_saveState.SaveStartTime(m_startTime);
        }

        // Stop timing the current session, write the session to the log, and clean up our save state
        private void StopTiming() {
            m_endTime = DateTime.Now;
            m_logFile.SaveLog(m_startTime, m_endTime, m_logMsg, m_category);
            m_saveState.CleanupSaveState();
        }
    }
}
