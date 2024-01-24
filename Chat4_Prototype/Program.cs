namespace Chat4_Prototype
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Server.AcceptMsg();
            }
            else
            {
                /*Thread tr2 = new Thread(() => { Client.ClientSendler(args[0]); });
                tr2.Start();
                Thread tr1 = new Thread(() => { Client.ClientListener(); });
                tr1.Start();*/
                await Client.ClientSendlerAync(args[0]);
                await Task.Run(() => { Client.ClientListener(); });


            }
        }
    }
}
