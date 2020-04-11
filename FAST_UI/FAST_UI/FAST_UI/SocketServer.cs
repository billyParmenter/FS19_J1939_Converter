/*
 * FILE          : SocketServer.cs
 * PROJECT       : FAST Dashboard
 * PROGRAMMER    : Mike Ramoutsakis, Billy Parmenter
 * FIRST VERSION : April 2, 2020
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace FAST_UI
{
    /*
     * NAME    : SocketServer
     * PURPOSE : The SocketServer class has been created to communicate
     *              over a socket. The SocketServer has members to create
     *              a socket, start listening, and receive messages over the socket
     */
    class SocketServer
    {
        public Socket socket;
        private int numberOfMessages = 0;
        private const int PORT_NO = 4040;
        private byte[] buffer = new byte[1024];

        public List<string> returnSock = new List<string>();//All entire messages

        /*
         * METHOD      : SocketServer
         * DESCRIPTION : Constructor calls Init function
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public SocketServer()
        {
            Init();
        }

        /*
        * METHOD      : Init
        * DESCRIPTION : creates a new socket to listen on
        * PARAMETERS  : NONE
        * RETURNS     : NONE
        */
        private void Init()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /*
        * METHOD      : StartListening
        * DESCRIPTION : Binds the created socket and starts listening for
        *               connections
        * PARAMETERS  : NONE
        * RETURNS     : NONE
        */
        public void StartListening()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT_NO));
            socket.Listen(500);
        }

        /*
        * METHOD      : Accept
        * DESCRIPTION : Accepts a connection over the socket then
        *               calls AcceptedCallback
        * PARAMETERS  : NONE
        * RETURNS     : NONE
        */
        public void Accept()
        {
            socket.BeginAccept(AcceptedCallback, null);
        }

        /*
        * METHOD      : AcceptedCallback
        * DESCRIPTION : Stores the connected client socket, the begins receiving on that
        *               connection
        * PARAMETERS  : IAsyncResult result
        * RETURNS     : NONE
        */
        private void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSock = socket.EndAccept(result);
            buffer = new byte[1024];
            clientSock.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clientSock);
            Accept();
        }

        /*
        * METHOD      : ReceivedCallback
        * DESCRIPTION : Once the message is received the message will be errorchecked and 
        *               stored for use on the dashboard
        * PARAMETERS  : IAsyncResult result
        * RETURNS     : NONE
        */
        private void ReceivedCallback(IAsyncResult result)
        {
            Socket clientSock = result.AsyncState as Socket;

            int buffSize;

            try
            {
                buffSize = clientSock.EndReceive(result);
            }
            catch
            {
                Accept();
                return;
            }



            byte[] packet = new byte[buffSize];
            Array.Copy(buffer, packet, packet.Length);

            //convert packet to string
            string tmp = string.Empty;
            tmp = Encoding.UTF8.GetString(packet, 0, packet.Length);

            if (string.IsNullOrEmpty(tmp) == false && string.IsNullOrWhiteSpace(tmp) == false && tmp != "" && tmp != null && tmp != "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0")
            {
                //store data packet
                storeData(tmp);
            }


            buffer = new byte[1024];
            Accept();
        }

        /*
        * METHOD      : storeData
        * DESCRIPTION : Stores the verified data to be used in the dashboard
        * PARAMETERS  : IAsyncResult result
        * RETURNS     : NONE
        */
        public void storeData(string data)
        {
            string[] parts = data.Split('\0');

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "")
                {
                    returnSock.Add(parts[i]);
                    numberOfMessages++;
                    Logger.Log(Logger.ErrorLevel.INFO, "Got message: " + parts[i]);
                    Logger.Log(Logger.ErrorLevel.INFO, "Number of messages: " + numberOfMessages);
                }
            }
        }

        
    }
}

