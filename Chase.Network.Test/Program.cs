// LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿
// LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. - All Rights Reserved
using Chase.Networking;
using Chase.Networking.Event;
using Newtonsoft.Json.Linq;

namespace Chase.Network.Test;

internal class Program
{
    private static async Task Main()
    {
        using NetworkClient client = new();
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.example.com");
        JObject? jObject = await client.GetAsJson(request);
        JArray? jArray = await client.GetAsJsonArray(request);

        DownloadProgressEvent progressEvent = (s, e) =>
        {
            Console.WriteLine($"Download Progress {e.Percentage:P2}%");
            Console.WriteLine($"Download Speed {e.BytesPerSecond}bps");
            Console.WriteLine($"Downloaded {e.BytesDownloaded}bytes / {e.TotalBytesToReceive}bytes");
            Console.WriteLine($"Remaining {e.BytesRemaining}bytes");
        };

        await client.DownloadFileAsync("https://example.com/download.zip", "/path/to/download.zip", progressEvent);
    }
}