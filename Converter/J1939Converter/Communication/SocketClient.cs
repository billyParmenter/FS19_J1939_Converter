using System;
using System.Net.Sockets;


namespace J1939Converter.Communication
{
    public class SocketClient
    {
        private string ip = "192.168.2.65";
        private int port = 4500;

        public SocketClient()
        {

        }

        public SocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }



        public void Send(string message)
        {
            string output;
            try
            {
                TcpClient client = new TcpClient(ip, port);

                Byte[] data = new Byte[256];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                data = new Byte[256];

                String responseData = String.Empty;

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                Console.WriteLine(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                Console.WriteLine(output);
            }
        }
    }
}
