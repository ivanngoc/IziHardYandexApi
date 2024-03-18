using System.Net.Sockets;

namespace IziHardGames.Libs.Networking.DeveloperTools
{

	public class TestPorts
	{
		public static void TestRange(int start, int end)
		{
			for (int i = start	; i < end; i++)
			{
				try
				{
					TcpListener tcpListener = new TcpListener(start);
					tcpListener.Start();
					Console.WriteLine($"{i} Free");
					tcpListener.Stop();
                }
				catch (Exception ex)
				{
					Console.WriteLine($"{i} Occupied");
				}
			}
		}
	} 
}