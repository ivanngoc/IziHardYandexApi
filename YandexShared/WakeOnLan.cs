using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IziHardGames.Libs.IoT.Controls
{
    public static class WakeOnLan
    {
        public const string myPC = "00-F1-F3-B2-01-97";
        public const string broPC = "70:8B:CD:54:93:53";
        public const string ipKem = "87.103.192.239";
        public const int portKemTest = 60301;
        public const int portKem = 6030;

        public static void SendSignal(string mac)
        {
            var data = MacToBytes(mac);
            using var client = new UdpClient();
            client.Send(data.Span, new IPEndPoint(IPAddress.Broadcast, 9));
        }

        public static async Task<string> SendSignalAsync(string mac, CancellationToken ct = default)
        {
            var data = MacToBytes(mac);
            using var client = new UdpClient();
            await client.SendAsync(data, new IPEndPoint(IPAddress.Broadcast, 9), ct);
            return "Turn On";
        }
        public static async Task<string> SendSignalLocalAsync(string ip, string mac)
        {
            try
            {
                var data = MacToBytes(mac);
                using var client = new UdpClient();
                await client.SendAsync(data, new IPEndPoint(IPAddress.Parse(ip), 9)).ConfigureAwait(false);
                return $"Succeed local call";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> SendSignalRemote(string ip, string mac)
        {
            var data = MacToBytes(mac);
            using var client = new UdpClient();
            try
            {
                await client.SendAsync(data, new IPEndPoint(IPAddress.Parse(ip), portKem), default).ConfigureAwait(false);
                return $"Succed WOL";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> SendSignalRemoteTcp(string ip, string mac)
        {
            var port = portKem;
            using (var client = new TcpClient())
            {
                var data = MacToBytes(mac);
                try
                {
                    await client.ConnectAsync(ip, port).ConfigureAwait(false);
                    await client.GetStream().WriteAsync(data, default).ConfigureAwait(false);
                    return $"Succed WOL";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        private static Memory<byte> MacToBytes(string mac)
        {
            Memory<byte> buf = new byte[102];
            Span<byte> data = buf.Span;

            data[..6].Fill(0xFF);
            for (var i = 0; i < 6; i++)
            {
                int startIndex = i * 3;
                data[i + 6] = byte.Parse(mac[startIndex..(startIndex + 2)], NumberStyles.HexNumber);
            }

            for (var i = 12; i < 102; i += 6)
            {
                data[6..12].CopyTo(data[i..(i + 6)]);
            }
            return buf;
        }
    }
}