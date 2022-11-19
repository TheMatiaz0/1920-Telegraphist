using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tracks
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private string trackKey;
        [SerializeField] private float scale = 1f;
        [SerializeField] private float offset;
        [SerializeField] private GameObject notePrefab;

        private List<Note> _notes;
        private AudioSource _audioSource;

        private Coroutine _spawningCoroutine;

        private List<GameObject> _noteObjects = new();
        
        private void Start()
        {
            _notes = TrackManager.Current.Tracks[trackKey];
            _audioSource = GetComponent<AudioSource>();

            StartSpawning();
        }

        private void StartSpawning()
        {
            _spawningCoroutine = StartCoroutine(Spawner());
            Invoke(nameof(PlayAudio), offset / scale);
        }

        private void PlayAudio()
        {
            _audioSource.Play();
        }

        private void Spawn(float duration)
        {
            var go = Instantiate(notePrefab, transform);
            go.transform.localScale = new Vector3(1, duration * scale, 1);
            _noteObjects.Add(go);
        }
        
        private IEnumerator Spawner()
        {
            foreach (var note in _notes)
            {
                yield return new WaitForSeconds(note.StartDeltaTime);
                Spawn(note.Duration);
                yield return new WaitForSeconds(note.Duration);
            }
        }

        private void Update()
        {
            foreach (var go in _noteObjects)
            {
                go.transform.Translate(new Vector3(0, -Time.deltaTime * scale, 0));
            }
        }
    }
}