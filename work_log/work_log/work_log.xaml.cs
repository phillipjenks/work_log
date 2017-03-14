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

    public partial class MainWindow : Window {

        private CLogger m_logger = new CLogger();

        public MainWindow() {
            InitializeComponent();

            m_logger.Init();

            HandleStateChange(); 
        }

        private void start_stop_button_Click(object sender, RoutedEventArgs e) {
            HandleButtonClick();
        }

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

        private bool TryGetMessage() {

            String msg = msg_txt.Text.Trim(' ');
            if(msg.Length <= 0) {
                return false;
            }

            m_logger.SetMessage(msg);

            return true;
        }

        private void HandleIdle() {
            main_label.Content = "Click The Start Button To Start A Session";
            start_stop_button.Content = "Start";
            msg_txt.Visibility = System.Windows.Visibility.Hidden;
            log_msg.Visibility = System.Windows.Visibility.Hidden;
        }
        private void HandleTiming() {
            main_label.Content = "Started Timing " + m_logger.StartTime;
            start_stop_button.Content = "Stop";
            msg_txt.Visibility = System.Windows.Visibility.Visible;
            log_msg.Visibility = System.Windows.Visibility.Visible;
            log_msg.Content = "Enter Log Message";
        }
        private void HandleFinished() {
            main_label.Content = "Started: " + m_logger.StartTime + "\tFinished: " + m_logger.EndTime;
            start_stop_button.Content = "Start";
            log_msg.Content = m_logger.LogMessage;
            msg_txt.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
