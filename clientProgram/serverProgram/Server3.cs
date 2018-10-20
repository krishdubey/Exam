using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace serverProgram
{
    class Server3
    {
        static Socket sck;
        static Socket acc;
        static int port = 9999;
        static IPAddress ip;
        static Thread rec;
        static string name;

        static string GetIp()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }

        static void recV()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] Buffer = new byte[255];
                int rec = acc.Receive(Buffer, 0, Buffer.Length, 0);
                Array.Resize(ref Buffer, rec);
                Console.WriteLine(Encoding.Default.GetString(Buffer));
            }
        }

        static void Main(string[] args)
        {
            rec = new Thread(recV);
            Console.WriteLine("your local Ip is : " + GetIp());
            Console.WriteLine("please enter your name");
            name = Console.ReadLine();
            Console.WriteLine("please enter your Host Port");
            string inputport = Console.ReadLine();
            try
            {
                port = Convert.ToInt32(inputport);
            }
            catch
            {
                port = 9999;
            }
            ip = IPAddress.Parse(GetIp());
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(ip, port));
            sck.Listen(0);
            acc = sck.Accept(); // acc is our locally imedialtely made socket
            rec.Start(); // starting thread

            while (true)
            {
                byte[] sdata = Encoding.Default.GetBytes("<" + name + ">" + Console.ReadLine());
                acc.Send(sdata, 0, sdata.Length, 0);
                //at the place of zero at the end - socketflag.none.
            }
        }
    }
}
