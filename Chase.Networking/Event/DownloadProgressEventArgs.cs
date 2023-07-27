/*
Chase.Networking LFInteractive LLC. 2021-2024﻿
a simple networking library
https://github.com/dcmanProductions/Chase.Networking
Licensed under GNU General Public License v3.0
https://www.gnu.org/licenses/gpl-3.0.en.html
*/

namespace Chase.Networking.Event;

/// <summary>
/// Runs when a download is initialized. <br/> See: <seealso
/// cref="NetworkClient.DownloadFileAsync(string, string, Chase.Networking.Event.DownloadProgressEvent?)">DownloadFileAsync</seealso>
/// </summary>
/// <param name="sender">
/// The <seealso cref="NetworkClient">NetworkClient</seealso> that called this event
/// </param>
/// <param name="args">The resulting arguements</param>
public delegate void DownloadProgressEvent(NetworkClient sender, DownloadProgressEventArgs args);

/// <summary>
/// Contains information about a download
/// </summary>
public class DownloadProgressEventArgs
{
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

    internal DownloadProgressEventArgs(double percentage, long bytesDownloaded, long totalBytesToReceive, double bytesPerSecond)
    {
        Percentage = percentage;
        BytesDownloaded = bytesDownloaded;
        TotalBytesToReceive = totalBytesToReceive;
        BytesRemaining = TotalBytesToReceive - BytesDownloaded;
        BytesPerSecond = bytesPerSecond;
    }
}