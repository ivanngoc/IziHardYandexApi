using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IziHardGames.Libs.IoT.Controls;

namespace Function
{
    public class ItemRequest
    {
        public string httpMethod { get; set; }
        public string body { get; set; }
    }

    public class ItemResponse
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
        public string version { get; set; }
        public ResponseWrap response { get; set; }
        public ItemResponse(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }

    public class ResponseWrap
    {
        public string text { get; set; }
        public string tts { get; set; }
        public bool end_session { get; set; }
    }

    public class Handler
    {
        public async Task<ItemResponse> FunctionHandler(ItemRequest request)
        {
            await TestUdpMessage().ConfigureAwait(false);
            var result = await WakeOnLan.SendSignalRemote(WakeOnLan.ipKem, WakeOnLan.broPC).ConfigureAwait(false);
            await Task.Delay(100).ConfigureAwait(false);
            return new ItemResponse(200, result)
            {
                version = "1.0",
                response = new ResponseWrap()
                {
                    text = "Здравствуйте! Это мы, хороводоведы.",
                    tts = "Здравствуйте! Это мы, хоров+одо в+еды.",
                    end_session = true,
                }
            };
        }

        private static async Task TestUdpMessage()
        {
            using var client = new UdpClient();
            await client.SendAsync(Encoding.UTF8.GetBytes($"{DateTime.Now}, This is Test Udp Message"), new IPEndPoint(IPAddress.Parse(WakeOnLan.ipKem), WakeOnLan.portKem)).ConfigureAwait(false);
        }
    }
}