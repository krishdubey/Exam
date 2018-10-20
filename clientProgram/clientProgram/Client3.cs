using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace clientProgram
{
    class Client3
    {
        static string name = "";
        static int port = 9999;
        static IPAddress ip;
        static Socket sck;
        static Thread rec;

        static void recV()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] Buffer = new byte[255];
                int rec = sck.Receive(Buffer, 0, Buffer.Length, 0);
                Array.Resize(ref Buffer, rec);
                Console.WriteLine(Encoding.Default.GetString(Buffer));
            }
        }

        static void Main(string[] args)
        {
            rec = new Thread(recV);
            Console.WriteLine("please enter your name");
            name = Console.ReadLine();
            Console.WriteLine("please enter the ip of the server");
            ip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("please enter the Port");
            string inputport = Console.ReadLine();
            try
            {
                port = Convert.ToInt32(inputport);
            }
            catch
            {
                port = 9999;
            }

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(new IPEndPoint(ip, port));

            byte[] conmsg = Encoding.Default.GetBytes("<" + name + ">" + "connected");
            sck.Send(conmsg, 0, conmsg.Length, 0);
            rec.Start();

            while (sck.Connected)
            {
                byte[] sdata = Encoding.Default.GetBytes("<" + name + ">" + Console.ReadLine());
                sck.Send(sdata, 0, sdata.Length, 0);
            }
        }
    }
}
