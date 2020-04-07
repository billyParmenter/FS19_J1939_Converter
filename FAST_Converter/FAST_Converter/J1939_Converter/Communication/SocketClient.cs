/*
 * FILE          : SocketClient.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */


using J1939Converter.Support;
using System;
using System.Configuration;
using System.Net.Sockets;


namespace J1939Converter.Communication
{


    /*
     * NAME    : SocketClient
     * PURPOSE : The SocketClient class has been created as a means to communicate
     *              over a socket connection. The SocketClient has members to create
     *              a socket, test the connection, and send a message on the socket
     */
    public class SocketClient
    {
        private string ip;
        private int port;
        private int numberOfMessages = 0;
        private TcpClient client;
        private NetworkStream stream;
        public static bool stop = false;
        /*
         * METHOD      : SocketClient
         * DESCRIPTION : Constructor creates a socket client using values 
         *                  taken from app.config
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public SocketClient()
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Creating SocketClient...");
            if(int.TryParse(ConfigurationManager.AppSettings["MachinePort"], out int port))
            {
                stop = false;
                Init(ConfigurationManager.AppSettings["MachineIP"], port);
            }
            else
            {
                Logger.Log(Logger.ErrorLevel.FATAL, "Socket port number could not be parsed: " + ConfigurationManager.AppSettings["MachinePort"]);
                throw new Exception("Port number could not be parsed: " + ConfigurationManager.AppSettings["MachinePort"]);
            }
            
        }





        /*
         * METHOD      : SocketClient
         * DESCRIPTION : Constructor creates a socket client using given 
         *                  values  
         * PARAMETERS  : string ip - The ip of the machine to connect to
         *               int port  - The port the machine is listening on
         * RETURNS     : NONE
         */
        public SocketClient(string ip, int port)
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Creating SocketClient...");
            stop = false;
            Init(ip, port);
        }

        private void Init(string ip, int port)
        {
            while (stop == false)
            {
                this.ip = ip;
                this.port = port;
                Logger.Log(Logger.ErrorLevel.INFO, "Creating TcpClient: ip=" + ip + " port=" + port);
                try
                {
                    client = new TcpClient(ip, port);

                    Logger.Log(Logger.ErrorLevel.INFO, "Creating NetworkStream");
                    stream = client.GetStream();
                    return;
                }
                catch (Exception e)
                {
                    Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to test SocketClient: ", e);
                    Logger.Log(Logger.ErrorLevel.INFO, "Trying again...");
                    Init(ip, port);
                }
            }
        }





        /*
         * METHOD      : Test
         * DESCRIPTION : Tests that the client is able to communicate over the 
         *                  given ip and port
         * PARAMETERS  : NONE
         * RETURNS     : string - the error message or null if no error
         */
        public string Test()
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Testing SocketClient...");

            try
            {
                Logger.Log(Logger.ErrorLevel.INFO, "Creating TcpClient: ip=" + ip + " port=" + port);

                TcpClient client = new TcpClient(ip, port);

                Logger.Log(Logger.ErrorLevel.INFO, "Creating NetworkStream");
                NetworkStream stream = client.GetStream();

                Logger.Log(Logger.ErrorLevel.INFO, "Closing stream and client");
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to test SocketClient: ",e);
                return e.Message;
            }
            catch (SocketException e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to test SocketClient: ", e);
                return e.Message;
            }

            return null;
        }





        /*
         * METHOD      : Send
         * DESCRIPTION : Sends a message over the socket
         * PARAMETERS  : string message - The message to send over the socket
         * RETURNS     : NONE
         */
        public void Send(string message)
        {
            Logger.Log(Logger.ErrorLevel.DEBUG, "Sending message over socket: " + message);

            try
            {


                byte[] data = new byte[256];
                Logger.Log(Logger.ErrorLevel.INFO, "Encoding data: " + data);
                data = System.Text.Encoding.ASCII.GetBytes(message + "\0");

                Logger.Log(Logger.ErrorLevel.INFO, "Writnig data to stream: data=" + data + " data length=" + data.Length);
                if (stream.CanWrite)
                {
                    stream.Write(data, 0, data.Length);

                }
                data = new byte[256];


                numberOfMessages++;
                Logger.Log(Logger.ErrorLevel.DEBUG, "Number of messages: " + numberOfMessages);

            }
            catch (Exception e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to send message: ", e);
                Init(ip, port);

            }
        }

        public static void Stop()
        {
            stop = true;
            Logger.Log(Logger.ErrorLevel.INFO, "Closing stream and client");
        }

        public void Close()
        {
            if (stream.CanRead == true)
            {
                stream.Close();
                client.Close();
            }
        }

    }
}
