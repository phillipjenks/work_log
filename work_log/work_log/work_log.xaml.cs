using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace work_log {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // Utility class for adding items to the combobox
    public class NameValue
    {
        private String dataValue;

        public NameValue(String val)
        {
            dataValue = val;
        }

        public String DataName
        {
            get { return dataValue; }
            set { dataValue = value; }
        }

        public String DataValue
        {
            get { return dataValue; }
            set { dataValue = value; }
        }

        public override string ToString()
        {
            return dataValue;
        }
    };

    public partial class MainWindow : Window {

        private CLogger m_logger = new CLogger();

        public MainWindow() {
            InitializeComponent();

            // Initialize the logger
            m_logger.Init();

            // Add the combo box categories
            combo_box.Items.Add(new NameValue("MISC"));
            combo_box.Items.Add(new NameValue("GAMEPLAY"));
            combo_box.Items.Add(new NameValue("INFRASTRUCTURE"));
            combo_box.Items.Add(new NameValue("UI"));
            combo_box.Items.Add(new NameValue("TRIGGER"));
            combo_box.Items.Add(new NameValue("AI"));
            combo_box.Items.Add(new NameValue("MP"));
            combo_box.Items.Add(new NameValue("EDITOR"));
            combo_box.Items.Add(new NameValue("UNREAL"));

            // Default to misc
            combo_box.SelectedIndex = 0;

            // Initialize the UI to the current logging state
            HandleStateChange(); 
        }

        // Event handler for clicks on the Start/Stop button
        private void start_stop_button_Click(object sender, RoutedEventArgs e) {
            HandleButtonClick();
        }

        // Handle the start/stop click advancing state and sending data to the logger as necessary
        private void HandleButtonClick() {
            switch(m_logger.State) {
                case WorkLogState.STATE_IDLE:
                case WorkLogState.STATE_FINISHED:
                    m_logger.AdvanceState();
                    HandleStateChange();
                    break;
                case WorkLogState.STATE_TIMING: 
                    {
                        if(TryGetMessage()) {
                            m_logger.AdvanceState();
                            HandleStateChange();
                        } else {
                            MessageBox.Show("You must enter a log message!");
                        }
                    }
                    break;
            }
        }

        // Update the UI on a state change
        private void HandleStateChange() {
            switch (m_logger.State) {
                case WorkLogState.STATE_IDLE:
                    HandleIdle();
                    break;
                case WorkLogState.STATE_TIMING:
                    HandleTiming();
                    break;
                case WorkLogState.STATE_FINISHED:
                    HandleFinished();
                    break;
            }
        }

        // Get string from the UI and set the appropriate properties in the logger class
        // Returns true if a message was successfully retrieved and saved in the logger
        private bool TryGetMessage() {

            String msg = msg_txt.Text.Trim(' ');
            if(msg.Length <= 0) {
                return false;
            }

            m_logger.SetMessage(msg);

            m_logger.SetCategory(combo_box.Text);

            return true;
        }

        // Update the UI for the Idle state
        private void HandleIdle() {
            main_label.Content = "Click The Start Button To Start A Session";
            start_stop_button.Content = "Start";
            msg_txt.Visibility = System.Windows.Visibility.Hidden;
            log_msg.Visibility = System.Windows.Visibility.Hidden;
            combo_box.Visibility = System.Windows.Visibility.Hidden;
        }

        // Update the UI for the timing state
        private void HandleTiming() {
            main_label.Content = "Started Timing " + m_logger.StartTime;
            start_stop_button.Content = "Stop";
            msg_txt.Visibility = System.Windows.Visibility.Visible;
            log_msg.Visibility = System.Windows.Visibility.Visible;
            log_msg.Content = "Enter Log Message";
            msg_txt.Text = "";
            combo_box.Visibility = System.Windows.Visibility.Visible;
        }

        // Update the UI for the after timing state
        private void HandleFinished() {
            main_label.Content = "Started: " + m_logger.StartTime + "\tFinished: " + m_logger.EndTime;
            start_stop_button.Content = "Start";
            log_msg.Content = m_logger.LogMessage + " (" + m_logger.Category + ")";
            msg_txt.Visibility = System.Windows.Visibility.Hidden;
            combo_box.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
