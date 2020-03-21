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
            Init(ip, port);
        }

        private void Init(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
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
            Logger.Log(Logger.ErrorLevel.INFO, "Sending message over socket: " + message);

            try
            {
                Logger.Log(Logger.ErrorLevel.INFO, "Creating TcpClient: ip=" + ip + " port=" + port);

                TcpClient client = new TcpClient(ip, port);

                byte[] data = new byte[256];
                Logger.Log(Logger.ErrorLevel.INFO, "Encoding data: " + data);
                data = System.Text.Encoding.ASCII.GetBytes(message);
                Logger.Log(Logger.ErrorLevel.INFO, "Creating NetworkStream");
                NetworkStream stream = client.GetStream();
                Logger.Log(Logger.ErrorLevel.INFO, "Writnig data to stream: data=" + data + " data length=" + data.Length);
                stream.Write(data, 0, data.Length);

                data = new byte[256];

                Logger.Log(Logger.ErrorLevel.INFO, "Closing stream and client");
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to test SocketClient: ", e);
            }
            catch (SocketException e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to test SocketClient: ", e);
            }
        }
    }
}
