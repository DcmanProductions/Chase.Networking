// LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿ LFInteractive LLC. 2021-2024﻿
// LFInteractive LLC. - All Rights Reserved
using Chase.Networking;
using Newtonsoft.Json;

namespace Chase.Network.Test;

internal class Program
{
    private static void Main()
    {
        using NetworkClient client = new();
        Newtonsoft.Json.Linq.JObject? obj = client.GetAsJson("https://api.modrinth.com/v2/search?query=The Warp Mod&limit=2&index=relevance&offset=0&facets=[[\"versions:1.19.4\"],[\"project_type:mod\"],[\"categories:fabric\"]]").Result;
        Console.WriteLine(JsonConvert.SerializeObject(obj));
        Console.Read();
    }
}