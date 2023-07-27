// LFInteractive LLC. - All Rights Reserved
namespace Chase.Networking.Event;

public delegate void DownloadProgressEvent(object? sender, DownloadProgressEventArgs args);

public class DownloadProgressEventArgs
{
    internal DownloadProgressEventArgs(double percentage, long bytesDownloaded, long totalBytesToReceive, double bytesPerSecond)
    {
        Percentage = percentage;
        BytesDownloaded = bytesDownloaded;
        TotalBytesToReceive = totalBytesToReceive;
        BytesRemaining = TotalBytesToReceive - BytesDownloaded;
        BytesPerSecond = bytesPerSecond;
    }

    /// <summary>
    /// The number of bytes downloaded
    /// </summary>
    public long BytesDownloaded { get; private set; }

    /// <summary>
    /// The speed of the download in bytes per second
    /// </summary>
    public double BytesPerSecond { get; private set; }

    /// <summary>
    /// The number of bytes left to be downloaded
    /// </summary>
    public long BytesRemaining { get; private set; }

    /// <summary>
    /// The percentage that has been downloaded, range from 0-1
    /// </summary>
    public double Percentage { get; private set; }

    /// <summary>
    /// The total number of bytes that the file has
    /// </summary>
    public long TotalBytesToReceive { get; private set; }
}