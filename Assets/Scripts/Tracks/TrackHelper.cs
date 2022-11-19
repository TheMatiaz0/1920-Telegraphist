using System.Collections.Generic;
using Newtonsoft.Json;
using Tracks;
using UnityEngine;

public static class TrackHelper
{
    public static Dictionary<string, List<Note>> LoadTracks(string str)
    {
        var data = JsonConvert.DeserializeObject<TrackList>(str);
        return data.Tracks;
    }
}