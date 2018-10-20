
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiConServ
{
    public class Server6
    {
        static Dictionary<string, ClientHandler> dSockets = new Dictionary<string, ClientHandler>();// 
        static int i = 1;
        static Socket sck;
        static Socket acc;

        static void Connection()
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.66"), 1234));
            sck.Listen(5);
        }

        static void Operation()
        {
            while (true)
            {
                // Accept the incoming request
                acc = sck.Accept(); // client ke open hote hi ye method call ho jayega
                try
                {

                    byte[] Buf = new byte[255];
                    int rec = acc.Receive(Buf, 0, Buf.Length, 0);
                    Array.Resize(ref Buf, rec); // method ke length ko resize kr dega
                    string dis = Encoding.Default.GetString(Buf);// byte me receive huya usko string me encode kiye hai
                    Console.WriteLine(dis + " Join"); // client open hoga to naam bhejega wahi pirnt kra dega
                    byte[] send = Encoding.Default.GetBytes(dis);// send me store krega dis jo bhi data hai
                    acc.Send(send, 0, send.Length, 0); //  client ko send krega
                    byte[] Buf1 = new byte[255];// 
                    int rec1 = acc.Receive(Buf1, 0, Buf1.Length, 0);
                    Array.Resize(ref Buf1, rec1);
                    string dis1 = Encoding.Default.GetString(Buf1);

                    ClientHandler mtch = new ClientHandler(acc, dis, dis1, dSockets);// clientHandler constructor hai
                    dSockets.Add(dis, mtch);// 

                    Thread t = new Thread(mtch.Run);
                    Thread t1 = new Thread(mtch.Send);
                    t.Start();
                    t1.Start();
                    i++;
                }
                catch
                {
                    Console.WriteLine("Hello");
                }
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("WelCome To Chat Server");
            Connection();
            Operation();

        }

    }
}