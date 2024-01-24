using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat4_Prototype
{
    internal class Client
    {
        public static async Task ClientSendlerAync(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5051);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                Console.Write("Введите имя получателя: ");
                string toName = Console.ReadLine();
                if (String.IsNullOrEmpty(toName))
                {
                    Console.WriteLine("Вы не ввели имя получателя.");
                    continue;
                }
                Console.Write("Введите сообщение (или 'Exit' для завершения): ");
                string text = Console.ReadLine();

                Message msg = new Message(name, text);
                msg.ToName = toName;
                string responseMsgJs = msg.ToJson();
                byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
                await udpClient.SendAsync(responseData, responseData.Length, ep);

                byte[] answerData = udpClient.Receive(ref ep);
                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                Message answerMsg = Message.FromJson(answerMsgJs);
                Console.WriteLine(answerMsg.ToString());
                if (answerMsg.Text == "Exit")
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void ClientSendler(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5051);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                Console.Write("Введите имя получателя: ");
                string toName = Console.ReadLine();
                if (String.IsNullOrEmpty(toName))
                {
                    Console.WriteLine("Вы не ввели имя получателя.");
                    continue;
                }
                Console.Write("Введите сообщение (или 'Exit' для завершения): ");
                string text = Console.ReadLine();

                Message msg = new Message(name, text);
                msg.ToName = toName;
                string responseMsgJs = msg.ToJson();
                byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
                udpClient.Send(responseData, responseData.Length, ep);

                byte[] answerData = udpClient.Receive(ref ep);
                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                Message answerMsg = Message.FromJson(answerMsgJs);
                Console.WriteLine(answerMsg.ToString());
                if (answerMsg.Text == "Exit")
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void ClientListener()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5051);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                byte[] answerData = udpClient.Receive(ref ep);
                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                Message answerMsg = Message.FromJson(answerMsgJs);
                Console.WriteLine(answerMsg.ToString());
                if (answerMsg.Text == "Exit")
                {
                    Environment.Exit(0);
                }
            }
        }


        public static async Task SendMsg(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5051);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                Console.Write("Введите имя получателя: ");
                string toName = Console.ReadLine();
                if (String.IsNullOrEmpty(toName))
                {
                    Console.WriteLine("Вы не ввели имя получателя.");
                    continue;
                }
                Console.Write("Введите сообщение (или 'Exit' для завершения): ");
                string text = Console.ReadLine();

                // if (text.ToLower() == "exit")
                // break;

                Message msg = new Message(name, text);
                msg.ToName = toName;
                string responseMsgJs = msg.ToJson();
                byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
                await udpClient.SendAsync(responseData, responseData.Length, ep);

                byte[] answerData = udpClient.Receive(ref ep);
                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                Message answerMsg = Message.FromJson(answerMsgJs);
                Console.WriteLine(answerMsg.ToString());
                if (answerMsg.Text == "Exit")
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
