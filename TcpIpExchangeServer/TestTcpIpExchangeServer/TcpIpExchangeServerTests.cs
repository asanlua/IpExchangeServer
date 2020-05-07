using System;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using TcpIpExchangeServer;

namespace TestTcpIpExchangeServer
{
    public class TcpIpExchangeServerTests
    {
        private const int defaultPort = 51111;

        [Test]
        public void TestServerResponseForSingleRequest()
        {
            TcpIpExchangeServer.TcpIpExchangeServer server = new TcpIpExchangeServer.TcpIpExchangeServer(defaultPort);
            Thread serverThread = new Thread(server.StartServer);
            serverThread.Start();


            string response_1 = TcpIpExchangeClient.SendMessage("asl@tst.com/psl@tst.com/108.23.142.12");
            Console.WriteLine(response_1);
            Assert.AreEqual("NOT_FOUND".Trim(), response_1);
            
            string response_2 = TcpIpExchangeClient.SendMessage("psl@tst.com/asl@tst.com/107.13.142.12");
            Console.WriteLine(response_2);
            Assert.AreEqual("108.23.142.12".Trim(), response_2);
        }

        [Test]
        public void TestServerParserWithProperRequest()
        {
            string requestedMail, receivedMail, ip;
            TcpIpExchangeServer.TcpIpExchangeServer server = new TcpIpExchangeServer.TcpIpExchangeServer(defaultPort);
            TcpIpExchangeServer.TcpIpExchangeServer.GetEntryFromRequest("asl@tst.com/psl@tst.com/107.13.142.12", out requestedMail, out receivedMail, out ip);
            Assert.AreEqual("asl@tst.com",requestedMail);
            Assert.AreEqual("psl@tst.com",receivedMail);
            Assert.AreEqual("107.13.142.12",ip);
        }
    }
}
