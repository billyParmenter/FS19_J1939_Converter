/*
 * FILE          : FAST_UI.cs
 * PROJECT       : FAST Dashboard
 * PROGRAMMER    : Mike Ramoutsakis, Billy Parmenter
 * FIRST VERSION : March 13 2020
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FAST_UI
{
    /*
     * NAME    : FAST_UI
     * PURPOSE : The FAST_UI class has been created to handle all interactions 
     *              from the user with the Dashboard.
     */
    public partial class FAST_UI : Form
    {
        //Create an instance of the socket server
        private readonly SocketServer socketServer = new SocketServer();
        //create a dictionary of PGN's that holds a Tuple of the SPN's and Key values 
        private readonly Dictionary<int, Tuple<int, string>> pgnDictionary = new Dictionary<int, Tuple<int, string>>()
        {
            { 64671, new Tuple<int, string>(6642, "FuelLevel")},//
            { 64737, new Tuple<int, string>(1600, "FuelRate")},//
            { 65265, new Tuple<int, string>(84, "Speed")}
        };
        //stop value used to stop logging
        private bool stop = false;
        //initialize a list of SPN's
        private readonly List<SPN> spnList = new List<SPN>();
        //counter for SPN's
        private int pastSPNs = 0;


        /*
         * FUNCTION    : FAST_UI
         * DESCRIPTION : This is the default custructor, it calls the InitializeComponent method, 
         *                  and hides or shows all necessary buttons for the initial load of the
         *                  user interface
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public FAST_UI()
        {
            InitializeComponent();
            ConnectPanel.Show();
            SideMenuPanel.Hide();
            indicatorPanel.Hide();
            LiveDataPanel.Hide();
        }

        /*
         * FUNCTION    : FS_Butt_Click
         * DESCRIPTION : This is "Connect to Farming Simulator" button click on the main page
         *                  of the dashboard. This button will be used when connecting 
         *                  to Farming simulator as opposed to a CAN BUS
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void FS_Butt_Click(object sender, EventArgs e)
        {
            ConnectPanel.Hide();
            SideMenuPanel.Show();
        }

        /*
         * FUNCTION    : CAN_Butt_Click
         * DESCRIPTION : This is "Connect to CAN" button click on the main page
         *                  of the dashboard. This button will be used when connecting 
         *                  to a CAN BUS as opposed to a Farming Simulator
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void CAN_Butt_Click(object sender, EventArgs e)
        {
            ConnectPanel.Hide();
            SideMenuPanel.Show();
        }

        /*
         * FUNCTION    : TachButt_Click **Not Implemented**
         * DESCRIPTION : This is Tachometer button click on the left menu panel
         *                  of the dashboard. This button will be used when connecting 
         *                  wanting to view a live tachometer of a tractor in either
         *                  Farming Simulator or Directly from a CAN Bus
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void TachButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Hide();
            indicatorPanel.Height = TachButt.Height;
            indicatorPanel.Top = TachButt.Top;
        }

        /*
         * FUNCTION    : LiveDataButt_Click
         * DESCRIPTION : This is Live Data button click on the left menu panel
         *                  of the dashboard. This button will be used when Data 
         *                  Logging. This button click will also start the Socket
         *                  Server to begin accepting values from Farming Simulator
         *                  or the CAN Bus
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void LiveDataButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Show();
            indicatorPanel.Height = LiveDataButt.Height;
            indicatorPanel.Top = LiveDataButt.Top;
            LiveStartLogButt.Enabled = true;
            LiveStopLogButt.Enabled = false;

            socketServer.StartListening();
            socketServer.Accept();
        }

        /*
         * FUNCTION    : DiagnosticsButt_Click **Not Implemented**
         * DESCRIPTION : This is Diagnostics button click on the left menu panel
         *                  of the dashboard. This button will be used when trying
         *                  do diagnose engine codes or issues with your piece of equipment
         *                  much like an OBD tool
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void DiagnosticsButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Hide();
            indicatorPanel.Height = DiagnosticsButt.Height;
            indicatorPanel.Top = DiagnosticsButt.Top;
        }


        /*
         * FUNCTION    : LiveStartLogButt_Click 
         * DESCRIPTION : This is Start Logging button click on the Live Data pane
         *                  of the dashboard. This button will be used when starting to log 
         *                  live data to the graph from Farming Simulator or CAN Bus in real
         *                  time
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void LiveStartLogButt_Click(object sender, EventArgs e)
        {
            LiveStartLogButt.Enabled = false;
            LiveStopLogButt.Enabled = true;

            //Call this function to begin parsing out the CAN Messages coming over the socket
            StartMessageParsing();

            //set the stop value to false for live data on the graph
            stop = false;

            //Create and start a new thread to update the graph in real time while keeping the interface active
            Thread updateThread = new Thread(new ThreadStart(getChartValues))
            {
                IsBackground = true
            };
            updateThread.Start();

        }

        /*
         * FUNCTION    : LiveStopLogButt_Click 
         * DESCRIPTION : This is Stop Logging button click on the Live Data pane
         *                  of the dashboard. This button will be used when stopping the
         *                  live data graph
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void LiveStopLogButt_Click(object sender, EventArgs e)
        {
            LiveStartLogButt.Enabled = true;
            LiveStopLogButt.Enabled = false;
            stop = true;
        }

        /*
         * FUNCTION    : FAST_UI_FormClosing 
         * DESCRIPTION : This Function is used for graceful close of the 
         *                  user interface
         * PARAMETERS  : object sender, EventArgs e
         * RETURNS     : NONE
         */
        private void FAST_UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;
            Thread.Sleep(100);
        }

        /*
         * FUNCTION    : StartMessageParsing 
         * DESCRIPTION : This Function will create a new thread for parsing out the CAN Messages
         *                  received over the socket on a new thread
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void StartMessageParsing()
        {
            stop = false;
            Thread updateThread = new Thread(new ThreadStart(ParseMessage))
            {
                IsBackground = true
            };
            updateThread.Start();
        }

        /*
         * FUNCTION    : ParseMessage 
         * DESCRIPTION : This Function begin parsing out the CAN Messages
         *                  received over the socket
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void ParseMessage()
        {
            int pastMessages = 0;

            //while real time graph is still logging
            while (stop == false)
            {
                //store message in a list
                List<string> newMessages = socketServer.returnSock.GetRange(pastMessages, socketServer.returnSock.Count - pastMessages);
                
                //iterate through the list of messages
                foreach (string message in newMessages)
                {
                    //if the message is not empty begin handling
                    if (message != "" && message != null)
                    {
                        //split the message into PGN and Data
                        string[] parts = message.Split(' ');
                        string PGN = parts[0];
                        string data = parts[1];

                        //Using the PgnReader parse out the PGN received over the socket
                        int pgn = PgnReader.ParseString(PGN);
                        //get the SPN number and SPN key based on the PGN
                        int spnNumber = pgnDictionary[pgn].Item1;
                        string spnKey = pgnDictionary[pgn].Item2;

                        //Create a new SPN object and store the SPNNumber and SPNKey
                        SPN spn = new SPN
                        {
                            SpnNumber = spnNumber,
                            SpnKey = spnKey,
                        };

                        //Get the SPN from the Database
                        DALManager.GetSPN(ref spn);

                        //Set the SPN's value based on the data given
                        spn.SetValue(data);
                        //Add the SPN to the list
                        spnList.Add(spn);

                        //increment the counter for number of messages received
                        pastMessages++;
                    }
                }
            }

        }


        /*
         * FUNCTION    : getChartValues 
         * DESCRIPTION : This Function is used to invoke the updateChart 
         *                  method
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void getChartValues()
        {
            while (stop == false)
            {
                try
                {
                    Invoke((MethodInvoker)delegate { updateChart(); });
                }
                catch
                {

                }
            }
        }

        /*
         * FUNCTION    : updateChart 
         * DESCRIPTION : This Function is used to update the real time
         *                  chart in the correct series based on the spn
         *                  key and value received over the socket
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private void updateChart()
        {
            //get all received SPN's
            List<SPN> newSPNs = spnList.GetRange(pastSPNs, spnList.Count - pastSPNs);

            //iterate through each SPN in the list and add it to the chart
            foreach (SPN spn in newSPNs)
            {
                LiveDataChart.Series[spn.SpnKey].Points.AddY(spn.Value);
                pastSPNs++;
            }
            Thread.Sleep(10);

        }


        
    }
}
