using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tracks
{
    public class TrackManager : MonoSingleton<TrackManager>
    {
        [SerializeField] private TextAsset file;
        [SerializeField] private AudioSource[] sources;
        [SerializeField] private Track[] trackComponents;

        public AudioSource[] Sources => sources;
        
        public Dictionary<string, List<Note>> Tracks { get; private set; }
        public Track[] TrackComponents => trackComponents;

        public List<float> AccuracyList { get; private set; } = new();

        public int MaxCombo { get; set; }

        protected override void Awake()
        {
            base.Awake();

            Tracks = TrackHelper.LoadTracks(file.text);
        }
    }
}