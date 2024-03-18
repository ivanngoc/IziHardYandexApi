using System.Diagnostics;

namespace IziHardGames.Libs.IoT.Controls
{
    public static class TurnOffPC
    {
        public static async Task TurnOfAsync(string ip)
        {
            var psi = new ProcessStartInfo("shutdown", @$"/s /t 10 /m \\{ip}");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            var process = Process.Start(psi);
            await process!.WaitForExitAsync().ConfigureAwait(false);
        }
    }
}