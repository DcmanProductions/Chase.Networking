// LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿
// LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. - All Rights Reserved
using Chase.Networking.Event;
using Newtonsoft.Json.Linq;

namespace Chase.Networking;

public class NetworkClient : HttpClient
{
    public Task DownloadFileAsync(string address, string file, DownloadProgressEvent? progress = null) => DownloadFileAsync(new Uri(address), file, progress);

    /// <summary>
    /// Downloads a file using an address and output file
    /// </summary>
    /// <param name="address"></param>
    /// <param name="file"></param>
    /// <param name="progress"></param>
    /// <returns></returns>
    public async Task DownloadFileAsync(Uri address, string file, DownloadProgressEvent? progress = null)
    {
        using HttpResponseMessage response = await GetAsync(address, HttpCompletionOption.ResponseHeadersRead);
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