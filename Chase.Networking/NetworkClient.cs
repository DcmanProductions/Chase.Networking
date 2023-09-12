/*
Chase.Networking LFInteractive LLC. 2021-2024﻿
a simple networking library
https://github.com/dcmanProductions/Chase.Networking
Licensed under GNU General Public License v3.0
https://www.gnu.org/licenses/gpl-3.0.en.html
*/

using Chase.Networking.Event;
using Newtonsoft.Json.Linq;

namespace Chase.Networking;

/// <summary>
/// A modified <seealso cref="HttpClient">HttpClient</seealso> that adds additional features.
/// </summary>
public class NetworkClient : HttpClient
{
    /// <summary>
    /// Downloads a file using an address and output file
    /// </summary>
    /// <param name="address">the direct download link</param>
    /// <param name="path">the directory or absolute file path</param>
    /// <param name="progress"></param>
    /// <returns></returns>
    public Task DownloadFileAsync(string address, string path, DownloadProgressEvent? progress = null) => DownloadFileAsync(new Uri(address), path, progress);

    /// <summary>
    /// Downloads a file using an address and output file
    /// </summary>
    /// <param name="address">the direct download uri</param>
    /// <param name="path">the directory or absolute file path</param>
    /// <param name="progress"></param>
    /// <returns></returns>
    public async Task<string> DownloadFileAsync(Uri address, string path, DownloadProgressEvent? progress = null)
    {
        bool guessFileName = new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory);
        using HttpResponseMessage response = await GetAsync(address, HttpCompletionOption.ResponseHeadersRead);
        string? ContentDisposition = response.Content.Headers.ContentDisposition?.FileName;
        string file = guessFileName ? Path.Combine(path, string.IsNullOrWhiteSpace(ContentDisposition) ? Path.GetRandomFileName() : ContentDisposition) : path;

        using HttpContent content = response.Content;
        long contentLength = content.Headers.ContentLength.GetValueOrDefault();

        using Stream contentStream = await content.ReadAsStreamAsync();
        byte[] buffer = new byte[8192];
        int bytesRead;
        long totalBytesRead = 0;
        double bytesPerSecond = 0;
        long currentBytes = 0;

        System.Timers.Timer timer = new(1000) { Enabled = true, AutoReset = true };
        timer.Elapsed += (s, e) =>
        {
            bytesPerSecond = totalBytesRead - currentBytes;
            currentBytes = totalBytesRead;
        };
        timer.Start();
        using (FileStream fs = new(file, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
        {
            while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
            {
                await fs.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalBytesRead += bytesRead;
                progress?.Invoke(this, new DownloadProgressEventArgs((double)totalBytesRead / contentLength, totalBytesRead, contentLength, bytesPerSecond));
            }
        }
        timer.Stop();
        return file;
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URI and parses the response content as JSON
    /// object (JObject).
    /// </summary>
    /// <param name="uri">The URI to send the GET request to.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a JObject representing the parsed
    /// JSON response, or null if the response is null.
    /// </returns>
    public Task<JObject?> GetAsJson(string uri) => GetAsJson(new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new(uri) });

    /// <summary>
    /// Sends an HTTP request and parses the response content as JSON object (JObject).
    /// </summary>
    /// <param name="requestMessage">The HttpRequestMessage to send.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a JObject representing the parsed
    /// JSON response, or null if the response is null.
    /// </returns>
    public async Task<JObject?> GetAsJson(HttpRequestMessage requestMessage)
    {
        using HttpResponseMessage response = await SendAsync(requestMessage);

        if (response != null)
        {
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        return null;
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URI and parses the response content as JSON array (JArray).
    /// </summary>
    /// <param name="uri">The URI to send the GET request to.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a JArray representing the parsed
    /// JSON response, or null if the response is null.
    /// </returns>
    public Task<JArray?> GetAsJsonArray(string uri) => GetAsJsonArray(new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new(uri) });

    /// <summary>
    /// Sends an HTTP request and parses the response content as JSON array (JArray).
    /// </summary>
    /// <param name="requestMessage">The HttpRequestMessage to send.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a JArray representing the parsed
    /// JSON response, or null if the response is null.
    /// </returns>
    public async Task<JArray?> GetAsJsonArray(HttpRequestMessage requestMessage)
    {
        using HttpResponseMessage response = await SendAsync(requestMessage);

        if (response != null)
        {
            return JArray.Parse(await response.Content.ReadAsStringAsync());
        }

        return null;
    }
}