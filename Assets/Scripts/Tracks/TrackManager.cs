using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tracks
{
    public class TrackManager : MonoSingleton<TrackManager>
    {
        [SerializeField] private TextAsset file;
        
        public Dictionary<string, List<Note>> Tracks { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Tracks = TrackHelper.LoadTracks(file.text);
        }
    }
}