using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TcpIpExchangeServer
{
    public class TcpIpExchangeServer
    {

        private TcpListener listener;
        private int port;
        Dictionary<string,string> map = new Dictionary<string,string>();

        public TcpIpExchangeServer(int port)
        {
            this.listener = new TcpListener(IPAddress.Any, port);
        }

        public void StartServer()
        {

            try
            {
                listener.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.WriteLine("SERVER Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("SERVER Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("SERVER: request received: {0}", data);

                        string strServerResponse = "ERROR";

                        try
                        {
                            GetEntryFromRequest(data,  out string requestedMail,  out string receivedMail,  out string ip);
                            map.Add(receivedMail,ip);

                            if (map.TryGetValue(requestedMail, out strServerResponse))
                            {
                                Console.WriteLine("Requested mail: {0} IP found: {1}", requestedMail, ip);
                            }
                            else
                            {
                                strServerResponse = "NOT_FOUND";
                            }
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(strServerResponse);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                listener.Stop();
            }
        }

        public static void GetEntryFromRequest(string request, out string requestedMail, out string receivedMail, out string ip)
        {
            
            requestedMail = null;
            receivedMail = null;
            ip = null;
            
            string[] elements = request.Split("/");
            if (elements.Length != 3)
            {
                Console.WriteLine("Unexpected length in parsed request: {0}", request);
                throw new Exception("Unexpected length in parsed request");
            }

            requestedMail = elements[0];
            receivedMail = elements[1];
            ip = elements[2];

        }
    }
}