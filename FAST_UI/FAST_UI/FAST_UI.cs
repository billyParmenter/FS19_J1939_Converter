using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FAST_UI
{
    public partial class FAST_UI : Form
    {
        private readonly SocketServer socketServer = new SocketServer();
        private readonly Dictionary<int, Tuple<int, string>> pgnDictionary = new Dictionary<int, Tuple<int, string>>()
        {
            { 64671, new Tuple<int, string>(6642, "FuelLevel")},//
            { 64737, new Tuple<int, string>(1600, "FuelRate")},//
            { 65265, new Tuple<int, string>(84, "Speed")}
        };
        private bool stop = false;
        private readonly List<SPN> spnList = new List<SPN>();
        private int pastSPNs = 0;
        public FAST_UI()
        {
            InitializeComponent();
            ConnectPanel.Show();
            SideMenuPanel.Hide();
            indicatorPanel.Hide();
            LiveDataPanel.Hide();
        }

        private void FS_Butt_Click(object sender, EventArgs e)
        {
            ConnectPanel.Hide();
            SideMenuPanel.Show();
        }

        private void CAN_Butt_Click(object sender, EventArgs e)
        {
            ConnectPanel.Hide();
            SideMenuPanel.Show();
        }

        private void TachButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Hide();
            indicatorPanel.Height = TachButt.Height;
            indicatorPanel.Top = TachButt.Top;
        }

        private void LiveDataButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Show();
            indicatorPanel.Height = LiveDataButt.Height;
            indicatorPanel.Top = LiveDataButt.Top;

            /*
             * 
             *  Thread for message parsing
             * 
             *      For each message
             *          split PGN and DATA
             *          Get SPN from database
             *          Get value based on SPN info
             *          Return value and key
             * 
             */

            //************************************This will start the socket listener*******************************//
            socketServer.StartListening();
            socketServer.Accept();
            StartMessageParsing();

            //*************************************split the message into CANID and Message*************************//
            //string[] splitData = socketServer.returnSock.Split(' ');

            //string binary = PgnReader.ConvertHexToBinary(splitData[0]);

            //**************************************store the parts of the CANID***********************************//
            //List<KeyValuePair<string, int>> PGN_Parts = PgnReader.ParseBinary(binary);


            //*********************************Stuff above commented out to test the LiveStartLogButt_Click button*****************************

            //Need to send values to the LiveStartLogButt_Click when the button is clicked

            Console.ReadLine();
        }

        private void DiagnosticsButt_Click(object sender, EventArgs e)
        {
            indicatorPanel.Show();
            LiveDataPanel.Hide();
            indicatorPanel.Height = DiagnosticsButt.Height;
            indicatorPanel.Top = DiagnosticsButt.Top;
        }

        private void StartMessageParsing()
        {
            stop = false;
            Thread updateThread = new Thread(new ThreadStart(ParseMessage))
            {
                IsBackground = true
            };
            updateThread.Start();
        }

        private void ParseMessage()
        {
            int pastMessages = 0;

            while (stop == false)
            {
                List<string> newMessages = socketServer.returnSock.GetRange(pastMessages, socketServer.returnSock.Count - pastMessages);
                
                 
                foreach (string message in newMessages)
                {
                    if (message != "" && message != null)
                    {
                        string[] parts = message.Split(' ');
                        string PGN = parts[0];
                        string data = parts[1];

                        int pgn = PgnReader.ParseString(PGN);
                        int spnNumber = pgnDictionary[pgn].Item1;
                        string spnKey = pgnDictionary[pgn].Item2;

                        SPN spn = new SPN
                        {
                            SpnNumber = spnNumber,
                            SpnKey = spnKey,
                        };


                        DALManager.GetSPN(ref spn);

                        spn.SetValue(data);
                        spnList.Add(spn);

                        pastMessages++;
                    }
                }
            }

        }

        private void getChartValues()
        {
            while (stop == false)
            {
                Invoke((MethodInvoker)delegate { updateChart(); });
            }
        }

        private void updateChart()
        {
            List<SPN> newSPNs = spnList.GetRange(pastSPNs, spnList.Count - pastSPNs);


            foreach (SPN spn in newSPNs)
            {
                LiveDataChart.Series[spn.SpnKey].Points.AddY(spn.Value );
                pastSPNs++;
            }
            Thread.Sleep(10);

        }


        //Start logging Button starts the live graphing
        private void LiveStartLogButt_Click(object sender, EventArgs e)
        {
            stop = false;
            Thread updateThread = new Thread(new ThreadStart(getChartValues))
            {
                IsBackground = true
            };
            updateThread.Start();

        }

        private void LiveStopLogButt_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void FAST_UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;

        }
    }
}
