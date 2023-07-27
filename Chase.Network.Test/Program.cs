// LFInteractive LLC. - All Rights Reserved
using Chase.Networking;
using CLMath;

namespace Chase.Network.Test;

internal class Program
{
    private static void Main()
    {
        using NetworkClient client = new();
        Uri url = new("https://releases.ubuntu.com/22.04.2/ubuntu-22.04.2-desktop-amd64.iso");
        string outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ubuntu.iso");

        client.DownloadFileAsync(url, outputFile, (s, e) =>
        {
            Console.CursorLeft = 0;
            Console.Write($"{e.Percentage:p2} - {CLFileMath.AdjustedFileSize(e.BytesPerSecond)}/s");
        }).Wait();
    }
}