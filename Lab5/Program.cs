using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace Lab5
{
    class Program
    {
        private static UdpClient client = new UdpClient();
       // public static string ip_broad = IPAddress.Broadcast.ToString();
        static bool flag;
        static int clientport;
        static void setup()
        {
            clientport = 2544;
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, clientport);
            client.Client.Bind(ip);
            flag = true;

        }
        static async Task SendMsg(string msg) {
           // Console.WriteLine("Sending...");
            byte[] Datagram = Encoding.UTF8.GetBytes(msg);
            IPEndPoint ip_broad = new IPEndPoint(IPAddress.Broadcast, clientport);
            await client.SendAsync(Datagram, Datagram.Length, ip_broad);
        }
        static async Task RecMsg() {
            while (flag)
            {
                var data_rec  = await client.ReceiveAsync();
                Console.WriteLine("Receive from:");
                Console.WriteLine(Encoding.UTF8.GetString(data_rec.Buffer));
            }
        }
        static async Task Main(string[] args)
    {
            string name;
            string cmd;
            string msg;
            setup();
            Task Rec = RecMsg();
            
            Console.WriteLine("Hello!Enter your name:");
            name = Console.ReadLine();
            Console.WriteLine($"Welcome on board,{name}! You may write a message or wait:");
            while (flag)
            {
                cmd = Console.ReadLine();
                switch (cmd) {
                    case "exit":
                        Console.WriteLine("Closing chat...");
                        flag = false;
                        Environment.Exit(0);
                        break;
                    default:
                        msg=DateTime.Now.ToString() + "-" + name +":"+cmd;
                        //Console.WriteLine($"{msg}");
                        await SendMsg(msg);
                        break;
                }
            }
           // if (flag == false)
            await Rec;

        }
}
}
