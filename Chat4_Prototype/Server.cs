using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat4_Prototype
{
    internal class Server
    {
        private static bool exitRequested = false;
        static private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static private CancellationToken ct = cancellationTokenSource.Token;

        public static async Task AcceptMsg()
        {
            Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(5051);
            Console.WriteLine("Сервер ожидает сообщения. Для завершения нажмите клавишу...");

            // Запустим задачу для ожидания нажатия клавиши
            Task exitTask = Task.Run(() =>
            {
                Console.ReadKey();
                exitRequested = true;
            });

            while (!exitRequested)
            {

                var data = udpClient.Receive(ref ep);
                //byte[] buffer = data.Buffer;
                string data1 = Encoding.UTF8.GetString(data);

                Task taskClient = new Task(async () =>
                {
                    Message msg = Message.FromJson(data1);
                    Message responseMsg = new Message();
                    //
                    if (msg != null)
                    {

                        if (msg.ToName.Equals("Server"))
                        {
                            if (msg.Text.ToLower().Equals("register"))
                            {
                                if (clients.TryAdd(msg.FromName, ep))
                                {
                                    responseMsg = new Message("Server", $"Клиент {msg.FromName} добавлен.");
                                }
                            }
                            else if (msg.Text.ToLower().Equals("delete"))
                            {
                                clients.Remove(msg.FromName);
                                responseMsg = new Message("Server", $"Клиент {msg.FromName} удален.");
                            }
                            else if (msg.Text.ToLower().Equals("list"))
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                foreach (var client in clients)
                                {
                                    stringBuilder.Append(client.Key + "\n");
                                }
                                responseMsg = new Message("Server", $"Список клиентов: \n {stringBuilder.ToString()}.");
                            }
                        }
                        // Prototype(отправка сообщения всем пользователям с помощью клонирования)
                        else if (msg.ToName.ToLower().Equals("all"))
                        {
                            foreach (var client in clients)
                            {
                                if (client.Key != msg.FromName)
                                {
                                    Message cloneMessage = (Message)msg.Clone();
                                    cloneMessage.ToName = client.Key;
                                    string msgJs = msg.ToJson();
                                    byte[] bytes1 = Encoding.UTF8.GetBytes(msgJs);
                                    await udpClient.SendAsync(bytes1, bytes1.Length, client.Value);
                                }
                            }
                            responseMsg = new Message("Server", "Сообщение отправлено всем клиентам");
                        }
                        // Отправка всем клиентам без клонирования
                        /*else if (msg.ToName.ToLower().Equals("all"))
                        {
                            foreach (var client in clients)
                            {
                                msg.ToName = client.Key;
                                string msgJs = msg.ToJson();
                                byte[] bytes1 = Encoding.UTF8.GetBytes(msgJs);
                                await udpClient.SendAsync(bytes1, bytes1.Length, client.Value);
                            }
                            responseMsg = new Message("Server", "Сообщение отправлено всем клиентам");
                        }*/
                        else if (clients.TryGetValue(msg.ToName, out IPEndPoint? value))
                        {
                            string msgJs = msg.ToJson();
                            byte[] bytes1 = Encoding.UTF8.GetBytes(msgJs);
                            await udpClient.SendAsync(bytes1, bytes1.Length, value);
                            responseMsg = new Message("Server", $"Пользователю {msg.ToName} отправлено сообщение");
                        }
                        else
                        {
                            responseMsg = new Message("Server", $"Пользователь {msg.ToName} не существует");
                        }
                    }
                    Console.WriteLine(msg.ToString());

                    if (msg.Text?.ToLower()?.Trim() == "exit")
                    {
                        cancellationTokenSource.Cancel();
                        responseMsg = new Message("Server", "Exit");
                    }

                    string responseMsgJs = responseMsg.ToJson();
                    byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
                    await udpClient.SendAsync(responseData, responseData.Length, ep);
                }, ct);

                if (!taskClient.IsCanceled)
                {
                    taskClient.Start();
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            // Дождитесь завершения задачи по нажатию клавиши
            exitTask.Wait();
        }
    }
}
