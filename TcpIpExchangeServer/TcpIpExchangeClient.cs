using System;
using System.Net.Sockets;

namespace TcpIpExchangeServer
{
    public class TcpIpExchangeClient
    {
        public static string SendMessage(string message)
        {
            try 
            {
                TcpClient client = new TcpClient("localhost", 51111);
                
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);         

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
    
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("CLIENT Sent: {0}", message);         

                // Receive the TcpServer.response.
    
                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                // Close everything.
                stream.Close();         
                client.Close();
                return responseData;
            } 
            catch (ArgumentNullException e) 
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            } 
            catch (SocketException e) 
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return "CLIENT ERROR";
        }
    }
}