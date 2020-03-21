using J1939Converter.Communication;
using System.Configuration;
using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace FAST_Converter.ViewModel
{
   
    class SettingsViewModel : WrapperViewModel
    {
        // The command button declarations
        #region Comand declarations

        public ICommand SelectFile_Cmd { get; set; }
        public ICommand SelectFolder_Cmd { get; set; }
        public ICommand ClosePopup_Cmd { get; set; }
        public ButtonCommand SaveSettings_Btn { get; set; }

        #endregion  

        // The booleanian values used in the UI. 
        // Can be updated by the code or the user.
        #region Bool declarations

        private bool _databaseAvailable;
        public bool DatabaseAvailable
        {
            get { return _databaseAvailable; }
            set
            {
                if (_databaseAvailable == value) return;

                if (value == true)
                {
                    value = TestDatabase();
                }
                else 
                {
                    OpenPopup("With no database being used the values and messages being sent are not reliable.");
                }


                _databaseAvailable = value;
            }
        }

        private bool _farmSimAvailable;
        public bool FarmSimAvailable
        {
            get { return _farmSimAvailable; }
            set
            {
                if (_farmSimAvailable == value) return;

                _farmSimAvailable = value;
            }
        }

        private bool _popupIsOpen;
        public bool PopupIsOpen
        {
            get { return _popupIsOpen; }
            set
            {
                if (_popupIsOpen == value) return;
                _popupIsOpen = value;
                OnPropertyChanged("PopupIsOpen");
            }
        }
        #endregion

        // The string values used in the UI. 
        // Can be updated by the code or the user.
        #region String declarations

        private string _machineIP;
        public string MachineIP
        {
            get
            {
                return _machineIP;
            }
            set
            {
                OnPropertyChanged(ref _machineIP, value);
                SaveSettings_Btn.ExecuteChanged();
            }
        }

        private string _machinePort;
        public string MachinePort
        {
            get
            {
                return _machinePort;
            }
            set
            {
                OnPropertyChanged(ref _machinePort, value);
                SaveSettings_Btn.ExecuteChanged();
            }   
        }

        private string _modOutFolder;
        public string ModOutFolder
        {
            get
            {
                return _modOutFolder;
            }
            set
            {
                OnPropertyChanged(ref _modOutFolder, value);
                SaveSettings_Btn.ExecuteChanged();
            }
        }

        private string _spnFile;
        public string SpnFile
        {
            get
            {
                return _spnFile;
            }
            set
            {
                OnPropertyChanged(ref _spnFile, value);
                SaveSettings_Btn.ExecuteChanged();
            }
        }

        private string _requiredError_Msg;
        public string RequiredError_Msg
        {
            get
            {
                return _requiredError_Msg;
            }
            set
            {
                OnPropertyChanged(ref _requiredError_Msg, value);

            }
        }

        private string _popup_Msg;
        public string Popup_Msg
        {
            get
            {
                return _popup_Msg;
            }
            set
            {
                OnPropertyChanged(ref _popup_Msg, value);

            }
        }

        #endregion





        /*
         * FUNCTION    : SettingsViewModel
         * DESCRIPTION : The default constrictor, will initialize the button commands and 
         *                  load any available settings
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public SettingsViewModel()
        {
            SaveSettings_Btn = new ButtonCommand(Save, CheckSettings);
            SelectFile_Cmd = new RelayCommand(SelectFile);
            SelectFolder_Cmd = new RelayCommand(SelectFolder);
            ClosePopup_Cmd = new RelayCommand(ClosePopup);
            LoadSettings();
        }





        #region Save/Load settings



        /*
         * FUNCTION    : Save
         * DESCRIPTION : Saves the settings from the UI to the app.config and navigates to the
         *                  MainPageView
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public void Save()
        {
            if (TestSettings() == true)
            {
                SaveSettings();
                Navigate(new MainPageViewModel());
            }
        }





        /*
         * FUNCTION    : LoadSettings
         * DESCRIPTION : Loads settings from the app.config to the UI
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void LoadSettings()
        {
            MachineIP = ConfigurationManager.AppSettings["MachineIP"];
            MachinePort = ConfigurationManager.AppSettings["MachinePort"];
            SpnFile = ConfigurationManager.AppSettings["SpnFile"];
            ModOutFolder = ConfigurationManager.AppSettings["ModOutFolder"];
            DatabaseAvailable = ConvertBoolSetting("DatabaseAvailable");
            FarmSimAvailable = ConvertBoolSetting("FarmSimAvailable");
        }





        /*
         * FUNCTION    : ConvertBoolSetting
         * DESCRIPTION : Converts a string value to bool
         * PARAMETERS  : string boolSetting - the string to convert to bool
         * RETURNS     : bool - the converted value of booltSetting
         */
        private bool ConvertBoolSetting(string boolSetting)
        {
            string settingValue = ConfigurationManager.AppSettings[boolSetting];
            
            if (settingValue == "" || settingValue =="false")
            {
                return false;
            }

            return true;
        }





        /*
         * FUNCTION    : SaveSettings
         * DESCRIPTION : Saves all the settings on the page to the app.config file
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void SaveSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["MachineIP"].Value = MachineIP;
            config.AppSettings.Settings["MachinePort"].Value = MachinePort;
            config.AppSettings.Settings["SpnFile"].Value = SpnFile;
            config.AppSettings.Settings["ModOutFolder"].Value = ModOutFolder;
            config.AppSettings.Settings["DatabaseAvailable"].Value = ConvertBoolSetting(DatabaseAvailable);
            config.AppSettings.Settings["FarmSimAvailable"].Value = ConvertBoolSetting(FarmSimAvailable);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }





        /*
         * FUNCTION    : SaveBoolSetting
         * DESCRIPTION : Converts the given bool to a string
         * PARAMETERS  : bool appSetting - the value to convert
         * RETURNS     : string - the string value of appSetting
         */
        private string ConvertBoolSetting(bool boolSetting)
        {
            if (boolSetting == true)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        #endregion

        #region Check required values are not empty



        /*
         * FUNCTION    : CheckSettings
         * DESCRIPTION : Checks that all required fields are filled and generates a string 
         *                  to be displayed to the user, telling them what values are still 
         *                  needed to be filled
         * PARAMETERS  : NONE
         * RETURNS     : bool - if all required fields are filled
         */
        private bool CheckSettings()
        {
            StringBuilder RequiredFields = new StringBuilder();

            CheckRequired(ref RequiredFields, MachineIP, "Machine IP");
            CheckRequired(ref RequiredFields, MachinePort, "Machine Port");
            CheckRequired(ref RequiredFields, ModOutFolder, "Mod output file");
            CheckRequired(ref RequiredFields, SpnFile, "SPN file");

            if (RequiredFields.Length != 0)
            {
                RequiredFields.Insert(0, "Reqired: ");
                RequiredError_Msg = RequiredFields.ToString();
                return false;
            }
            
            RequiredError_Msg = "";
            

            return true;
        }





        /*
         * FUNCTION    : CheckRequired
         * DESCRIPTION : 
         * PARAMETERS  : StringBuilder RequiredFields -  the string builder to add\append to
         *               string field - the field to check
         *               string name  - the name of the field to add to the string builder
         * RETURNS     : NONE
         */
        private void CheckRequired(ref StringBuilder RequiredFields, string field, string name)
        {
            if (field.Trim().Length == 0)
            {
                if (RequiredFields.Length != 0)
                {
                    RequiredFields.Append(", ");
                }

                RequiredFields.Append(name);
            }
        }

        #endregion

        #region Check setting values are valid



        /*
         * FUNCTION    : TestSettings
         * DESCRIPTION : Tests the database and the socket settings
         * PARAMETERS  : NONE
         * RETURNS     : bool - true if tests pass
         */
        private bool TestSettings()
        {
            if (TestSocket() == false)
            {
                return false;
            }

            if (DatabaseAvailable == true && TestDatabase() == false)
            {
                return false;
            }

            return true;
        }





        /*
         * FUNCTION    : TestDatabase
         * DESCRIPTION : Tests the database settings
         * PARAMETERS  : NONE
         * RETURNS     : bool - true if test passes
         */
        private bool TestDatabase()
        {
            string databaseResult = Database.Test();

            if(databaseResult != null)
            {
                OpenPopup(databaseResult);
                return false;
            }

            return true;
        }





        /*
         * FUNCTION    : TestSocket
         * DESCRIPTION : Tests the socket settings
         * PARAMETERS  : NONE
         * RETURNS     : bool - true if test passes
         */
        private bool TestSocket()
        {
            if (IPAddress.TryParse(MachineIP, out _) == false)
            {
                OpenPopup("Given IP not valid");
                return false;
            }

            if (int.TryParse(MachinePort, out int port) == false)
            {
                OpenPopup("Given port not valid");
                return false;
            }

            SocketClient socket = new SocketClient(MachineIP, port);

            string socketResult = null;//socket.Test();

            if(socketResult != null)
            {
                OpenPopup("Could not connect to socket " + socketResult);
                return false;
            }

            return true;

        }

        #endregion

        #region Select files/folders





        /*
         * FUNCTION    : SelectFile
         * DESCRIPTION : Opens the file explorer to allow the user to select a file
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text|*.txt|All|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                SpnFile = openFileDialog.FileName;
            }
        }





        /*
         * FUNCTION    : SelectFolder
         * DESCRIPTION : Opens the file explorer to allow the user to select a folder
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ModOutFolder = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion

        #region Popup window open/close





        /*
         * FUNCTION    : ClosePopup
         * DESCRIPTION : Closes the popup window
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void ClosePopup()
        {
            PopupIsOpen = false;
        }





        /*
         * FUNCTION    : OpenPopup
         * DESCRIPTION : Opens the popupwindow and displays the message
         * PARAMETERS  : string msg - the message to display on the popup window
         * RETURNS     : NONE
         */
        private void OpenPopup(string msg)
        {
            Popup_Msg = msg;
            PopupIsOpen = true;
        }

        #endregion

    }
}
