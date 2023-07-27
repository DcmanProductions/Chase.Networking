# Chase.Networking

a simple networking library

## Create Client

```csharp
using Chase.Networking;
// ...
using NetworkClient client = new NetworkClient();
```

## Download File

```csharp
client.DownloadFileAsync("https://example.com/download.zip", "/path/to/download.zip");
```

### Getting additional download information

Additional information that can be received is as follows:
- `Bytes Downloaded` - The total amount of bytes that have been downloaded,
- `Bytes Per Second` - The speed of the download in bps,
- `Bytes Remaining` - The number of bytes that are left to be downloaded,
- `Percentage` - The percent of the file that has been downloaded, value is a float between 0 and 1,
- `Total Bytes To Receive` - The total size of the downloading file,

```csharp
DownloadProgressEvent progressEvent = (s, e) =>
{
    Console.WriteLine($"Download Progress {e.Percentage:P2}%");
    Console.WriteLine($"Download Speed {e.BytesPerSecond}bps");
    Console.WriteLine($"Downloaded {e.BytesDownloaded}bytes / {e.TotalBytesToReceive}bytes");
    Console.WriteLine($"Remaining {e.BytesRemaining}bytes");
};

client.DownloadFileAsync("https://example.com/download.zip", "/path/to/download.zip", progressEvent);
```

## Get Request as Newtonsoft.JSON

A normal get request
```csharp
JObject? jObject = await client.GetAsJson("https://api.example.com");
JArray? jArray = await client.GetAsJsonArray("https://api.example.com");
```

or you can also pass a `HttpRequestMessage` for more advanced requests
```csharp
using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.example.com");
JObject? jObject = await client.GetAsJson(request);
JArray? jArray = await client.GetAsJsonArray(request);
```

