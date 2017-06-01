using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandsLib;
using DataModel;
using System.Net.Sockets;
using CommunicationSerializer;

namespace TCPClient
{
    public static class TCPRPCClient
    {
        private static TcpClient Client { get; } = new TcpClient();
        private static NetworkStream Stream { get; set; }
        private static CommunicationSerializer.CommunicationSerializer Serializer { get; } = new CommunicationSerializer.CommunicationSerializer();

        public static bool IsConnected { get { return Client.Connected; } }

        public static bool Connect(string remoteIp, int port)
        {
            try
            {
                Client.Connect(remoteIp, port);
                Stream = Client.GetStream();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static void Send(string message)
        {
            if (!IsConnected) return;
            var buffer = Encoding.UTF8.GetBytes(message);
            Stream.Write(buffer, 0, buffer.Length);
        }

        private static string Receive()
        {
            string message = "";
            if (!IsConnected) return message;
            var buffer = new byte[256];
            while (Stream.DataAvailable || message.Length == 0)
            {
                int bytesCount = Stream.Read(buffer, 0, buffer.Length);
                message += Encoding.UTF8.GetString(buffer, 0, bytesCount);
            }
            return message;
        }

        private static object RemoteProcedureCall(ICustomCommand command)
        {
            string jsoncommand = Serializer.Serialize(command);
            Send(jsoncommand);
            string jsonresponse = Receive();
            Response response = Serializer.Deserialize(jsonresponse);
            if (response.Exception != null) throw response.Exception;
            return response.Value;
        }

        public static int AddUser(string userName)
        {
            return Convert.ToInt32(RemoteProcedureCall(new AddUserCommand { Name = userName }));
        }
    }
}
