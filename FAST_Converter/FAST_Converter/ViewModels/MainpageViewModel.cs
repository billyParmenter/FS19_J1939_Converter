/*
 * FILE          : MainPageViewModel.cs
 * PROJECT       : FAST_Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : March 20, 2020
 */


using J1939Converter;
using J1939Converter.Support;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;


namespace FAST_Converter.ViewModel
{
    /*
     * NAME    : MainPageViewModel
     * PURPOSE : The MainPageViewModel class has been created to allow the 
     *              user to start, stop, and view the messages being sent
     */
    internal class MainPageViewModel : WrapperViewModel
    {

        private static Converter converter = new Converter();
        private readonly object _messageInfosLock = new object();


        private static bool _stopped;
        public bool Stopped
        {
            get { return _stopped; }
            set
            {
                if (_stopped == value)
                {
                    return;
                }
                _stopped = value;
                Stop_Cmd.ExecuteChanged();
                Start_Cmd.ExecuteChanged();
            }
        }

        private bool _started;
        public bool Started
        {
            get { return _started; }
            set
            {
                if (_started == value)
                {
                    return;
                }
                _started = value;
                Stop_Cmd.ExecuteChanged();
                Start_Cmd.ExecuteChanged();
            }
        }


        //This is the list of messages to be shown on the UI in the listView
        private ObservableCollection<MessageInfo> _messageInfos;
        public ObservableCollection<MessageInfo> MessageInfos
        {
            get => _messageInfos;
            set
            {
                _messageInfos = value;
                // Allows for updating the UI in a seperate thread
                BindingOperations.EnableCollectionSynchronization(_messageInfos, _messageInfosLock);
                OnPropertyChanged("MessageInfos");
            }
        }

        // Strings that are displayed on the UI but are updated by code
        #region String declarations

        private string _data;
        public string Data
        {
            get => _data;
            set => OnPropertyChanged(ref _data, value);
        }

        private string _infoMsg;
        public string InfoMsg
        {
            get => _infoMsg;
            set => OnPropertyChanged(ref _infoMsg, value);
        }

        private string _verboseMsg;
        public string VerboseMsg
        {
            get => _verboseMsg;
            set => OnPropertyChanged(ref _verboseMsg, value);
        }

        #endregion

        public ButtonCommand Start_Cmd { get; set; }
        public ButtonCommand Stop_Cmd { get; set; }
        public RelayCommand Back_Cmd { get; set; }





        /*
         * FUNCTION    : MainPageViewModel
         * DESCRIPTION : The default constructor
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public MainPageViewModel()
        {
            MessageInfos = new ObservableCollection<MessageInfo>();
            Start_Cmd = new ButtonCommand(Start, EnableStart);
            Stop_Cmd = new ButtonCommand(Stop, EnableStop);
            Back_Cmd = new RelayCommand(Back);
        }

        private void Back()
        {
            if (Started == true)
            {
                Stop();
            }
            Navigate(new SettingsViewModel());
        }





        /*
         * FUNCTION    : Start
         * DESCRIPTION : Spawns a new thread to start doing the conversion
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void Start()
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Converter started, creating new thread");
            Stopped = false;
            Started = true;
            Thread test = new Thread(new ThreadStart(Convert));
            test.Start();
        }





        /*
         * FUNCTION    : Convert
         * DESCRIPTION : Runs untill the stop button is pressed. Will conn the converters 
         *                  DoConvert method to generate messages. The method will get the 
         *                  MessageInfos back to be added to the UIs list view
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void Convert()
        {
            Verbose("Started the system");
            Verbose("Waiting for connection...");
            converter.Init();
            ObservableCollection<MessageInfo> newMessageInfos = MessageInfos;
            if(Stopped == false)
            {
                Verbose("Connected!\nSending messages.");
            }

            while (Stopped == false)
            {

                Logger.Log(Logger.ErrorLevel.INFO, "Getting new data from FS mod");
                ObservableCollection<MessageInfo> tmpMessageInfos = converter.DoConvert();

                foreach (MessageInfo messageInfo in tmpMessageInfos)
                {
                    newMessageInfos.Add(messageInfo);
                }

                MessageInfos = newMessageInfos;
            }
        }





        /*
         * FUNCTION    : Stop
         * DESCRIPTION : Tells the thread to finish the last message and to stop.
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void Stop()
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Converter stopped");
            Verbose("Stopped the system");
            Stopped = true;
            Started = false;
            converter.Stop();
        }





        /*
         * FUNCTION    : OnWindowClosing
         * DESCRIPTION : Tells the thread to finish the last message and to stop when 
         *                  the window is closed 
         * PARAMETERS  : object sender, CancelEventArgs e
         * RETURNS     : NONE
         */
        internal static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _stopped = true;
            converter.Stop(true);
        }





        /*
         * FUNCTION    : Verbose
         * DESCRIPTION : Updates the verbose message on the UI
         * PARAMETERS  : string verbose - the message to add the the verbose message box
         * RETURNS     : NONE
         */
        public void Verbose(string verbose)
        {
            StringBuilder stringBuilder = new StringBuilder(VerboseMsg);

            stringBuilder.AppendLine(verbose);

            VerboseMsg = stringBuilder.ToString();
        }



        private bool EnableStart()
        {
            if(Stopped == false && Started == false)
            {
                return true;
            }

            return Stopped;
        }

        private bool EnableStop()
        {
            return Started;
        }
    }
}
