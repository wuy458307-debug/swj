using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkAdapters
{
    public class EthernetAdapter
    {
        private Socket _socket;
        private IPEndPoint _endPoint;

        public EthernetAdapter(string ipAddress, int port)
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            _socket.Connect(_endPoint);
        }

        public void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _socket.Send(data);
        }

        public string Receive()
        {
            byte[] buffer = new byte[1024];
            int received = _socket.Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, received);
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}