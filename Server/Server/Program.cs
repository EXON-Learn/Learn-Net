using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Console_Net
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"사용법 : {Process.GetCurrentProcess().ProcessName} <Bind IP>");
                return;
            }

            string data = null;

            string bindIp = args[0];
            const int bindPort = 5425;
            TcpListener server = null;

            try
            {
                IPEndPoint localAddress = new IPEndPoint(IPAddress.Parse(bindIp), bindPort);
                server = new TcpListener(localAddress);
                server.Start();

                Console.WriteLine("메아리 서버 시작");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"클라이언트 접속 : {((IPEndPoint)client.Client.RemoteEndPoint).ToString()} ");

                    NetworkStream stream = client.GetStream();

                    int length;
                    byte[] bytes = new byte[256];

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.Default.GetString(bytes, 0, length);
                        Console.WriteLine(String.Format("수신: {0}", data));

                        string allData = data;
                        if (data == "장승윤")
                        {
                            allData += "은 바보입니다.";
                        }
                        else if (data == "박시혁")
                        {
                            allData += "은 천재입니다.";
                        }
                        byte[] msg = Encoding.Default.GetBytes(allData);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine(String.Format("송신: {0}", allData));

                        
                    }

                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

                server.Stop();
            }


            Console.WriteLine("서버를 종료합니다.");
        }
    }
}
