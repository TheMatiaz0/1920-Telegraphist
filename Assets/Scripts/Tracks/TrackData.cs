using System;
using System.Collections.Generic;

namespace Tracks
{
    [Serializable]
    public class TrackList
    {
        public Dictionary<string, List<Note>> Tracks { get; set; }
    }
    

    [Serializable]
    public class Note
    {
        public float StartTime { get; set; }
        public float StartDeltaTime { get; set; }
        public float Duration { get; set; }
    }
}