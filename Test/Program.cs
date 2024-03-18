// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using IziHardGames.Libs.IoT.Controls;
using IziHardGames.Libs.Networking.DeveloperTools;

//TestPorts.TestRange(50000, 50100);
//await TurnOffPC.TurnOfAsync("192.168.0.5");
await WakeOnLan.SendSignalAsync(WakeOnLan.broPC);
//WakeOnLan.SendSignalLocalAsync("192.168.0.1", WakeOnLan.broPC);
Console.WriteLine("Hello, World!");
Console.ReadLine();
