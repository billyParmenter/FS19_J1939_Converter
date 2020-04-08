using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace FAST_UI
{
    class SocketServer
    {
        public Socket socket;
        private int numberOfMessages = 0;
        private const int PORT_NO = 4040;
        private byte[] buffer = new byte[1024];

        public List<string> returnSock = new List<string>();//All entire messages
        public SocketServer()
        {
            Init();
        }
        
        private void Init()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        public void StartListening()
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT_NO));
            socket.Listen(500);
        }
        

        public void Accept()
        {
            socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSock = socket.EndAccept(result);
            buffer = new byte[1024];
            clientSock.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clientSock);
            Accept();
        }

        //called when it gets the message
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
                

                //handle packet
                storeData(tmp);
            }


            buffer = new byte[1024];
            clientSock.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clientSock);
        }

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

