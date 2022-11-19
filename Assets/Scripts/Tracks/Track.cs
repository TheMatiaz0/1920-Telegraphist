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
        [SerializeField] private KeyCode keyCode;
        [SerializeField] private float threshold;

        [SerializeField] private GameObject go;

        private List<Note> _notes;
        private AudioSource _audioSource;

        private Coroutine _spawningCoroutine;

        private int _currentNoteIndex = 0;

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
            Invoke(nameof(OffsetStart), offset);
        }

        private void OffsetStart()
        {
            _audioSource.Play();
            StartCoroutine(Asdf());
        }

        private void Spawn(float duration)
        {
            var go = Instantiate(notePrefab, transform);
            go.transform.localPosition = new Vector3(0, offset * scale, 0);
            go.transform.localScale = new Vector3(1, duration * scale, 1);
            _noteObjects.Add(go);
        }
        
        private IEnumerator Spawner()
        {
            foreach (var (note, i) in _notes.Select((x, i) => (x, i)))
            {
                yield return new WaitForSeconds(note.StartDeltaTime + (note.Duration / 2));
                Spawn(note.Duration);
                yield return new WaitForSeconds(note.Duration - (note.Duration / 2));
            }
        }

        private IEnumerator Asdf()
        {
            foreach (var (note, i) in _notes.Select((x, i) => (x, i)))
            {
                yield return new WaitForSeconds(note.StartDeltaTime - (note.Duration / 2));
                _currentNoteIndex = i;
                yield return new WaitForSeconds(note.Duration * 1.5f);
            }
        }

        private void Update()
        {
            foreach (var go in _noteObjects)
            {
                go.transform.localPosition += new Vector3(0, -Time.deltaTime * scale, 0);
            }
            
            HandleInput();
        }

        private float _accuracy;
        private bool _holding;
        private int _currentNoteIndexForInput;
        private void HandleInput()
        {
            var note = _notes[_holding ? _currentNoteIndexForInput : _currentNoteIndex];
            var time = _audioSource.time;
            
            if (Input.GetKeyDown(keyCode))
            {
                var dist = Mathf.Abs(note.StartTime - time);
                if (dist < threshold)
                {
                    _accuracy += 1 - (dist / threshold);
                }

                _currentNoteIndexForInput = _currentNoteIndex;
                _holding = true;
            } else if (Input.GetKeyUp(keyCode))
            {
                var dist = Mathf.Abs(note.StartTime + note.Duration - time);
                if (dist < threshold)
                {
                    _accuracy += 1 - (dist / threshold);
                }
                
                NoteEnd(_accuracy / 2);
                
                _accuracy = 0;
                _holding = false;
            }
        }

        private void NoteEnd(float accuracy)
        {
            Debug.Log($"accuracy: {accuracy}");
        }
    }
}