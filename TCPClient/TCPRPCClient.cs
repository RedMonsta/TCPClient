using System;
using System.Text;
using CommandsLib;
using System.Net.Sockets;

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

        public static int AddAddress(string city, string street, int build, int flat)
        {
            return Convert.ToInt32(RemoteProcedureCall(new AddAddressCommand { City = city, Street = street, Build = build, Flat = flat }));
        }

        public static int AddOrder(string goodName)
        {
            return Convert.ToInt32(RemoteProcedureCall(new AddOrderCommand { GoodName = goodName }));
        }


        public static bool RemoveUser(int userid)
        {
            return (bool)RemoteProcedureCall(new RemoveUserCommand { UserId = userid });
        }

        public static bool RemoveAddress(int addrid)
        {
            return (bool)RemoteProcedureCall(new RemoveAddressCommand { AddrId = addrid });
        }

        public static bool RemoveOrder(int ordid)
        {
            return (bool)RemoteProcedureCall(new RemoveOrderCommand { OrdId = ordid });
        }


        public static bool AddUserToAddress(int userid, int addrid)
        {
            return (bool)RemoteProcedureCall(new AddLinkUserToAddressCommand { UserId = userid, AddrId = addrid });
        }

        public static bool AddAddressToUser(int addrid, int userid)
        {
            return (bool)RemoteProcedureCall(new AddLinkAddressToUserCommand { UserId = userid, AddrId = addrid });
        }

        public static bool AddOrderToAddress(int ordid, int addrid)
        {
            return (bool)RemoteProcedureCall(new AddLinkOrderToAddressCommand { OrdId = ordid, AddrId = addrid });
        }

        public static bool AddOrderToUser(int ordid, int userid)
        {
            return (bool)RemoteProcedureCall(new AddLinkOrderToUserCommand { UserId = userid, OrdId = ordid });
        }


        public static bool RemoveUserFromAddress(int userid, int addrid)
        {
            return (bool)RemoteProcedureCall(new RemoveLinkUserToAddressCommand { UserId = userid, AddrId = addrid });
        }

        public static bool RemoveAddressFromUser(int addrid, int userid)
        {
            return (bool)RemoteProcedureCall(new RemoveLinkAddressToUserCommand { UserId = userid, AddrId = addrid });
        }

        public static bool RemoveOrderFromAddress(int ordid, int addrid)
        {
            return (bool)RemoteProcedureCall(new RemoveLinkOrderToAddressCommand { OrdId = ordid, AddrId = addrid });
        }

        public static bool RemoveOrderFromUser(int ordid, int userid)
        {
            return (bool)RemoteProcedureCall(new RemoveLinkOrderToUserCommand { UserId = userid, OrdId = ordid });
        }


        public static string GetData()
        {
            return (string)RemoteProcedureCall(new GetDataCommand());
        }
    }
}
