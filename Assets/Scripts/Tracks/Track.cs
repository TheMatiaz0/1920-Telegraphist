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

        private bool _started = false;
        
        
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
            StartCoroutine(UpdateCurrentIndex());
            _started = true;
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
                yield return new WaitForSeconds(note.Duration / 2);
            }
        }

        private IEnumerator UpdateCurrentIndex()
        {
            foreach (var (note, i) in _notes.Select((x, i) => (x, i)))
            {
                yield return new WaitForSeconds(note.StartDeltaTime + note.Duration);
                _currentNoteIndex = i + 1;
                if (!_holding && _currentNoteIndex > _finishedIndex && _currentNoteIndex - _finishedIndex > 1)
                {
                    NoteEnd(0);
                }
                // Debug.Log($"{i} available");
            }
        }

        private float _timer;
        private void Update()
        {
            foreach (var go in _noteObjects)
            {
                go.transform.localPosition += new Vector3(0, -Time.deltaTime * scale, 0);
            }
            
            HandleInput();
            
            if (!_started) return;

            _timer += Time.deltaTime;
        }

        private float _accuracy;
        private bool _holding;
        private int _currentNoteIndexForInput;
        private int _finishedIndex = -1;
        
        private void HandleInput()
        {
            var idx = _holding ? _currentNoteIndexForInput : _currentNoteIndex;

            if (idx >= _notes.Count) return;
            
            var note = _notes[idx];

            if (!_holding && _finishedIndex >= idx && Input.GetKeyDown(keyCode))
            {
                Debug.Log($"ERR, fin: {_finishedIndex}, idx: {idx}");
                return;
            }
            
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log($"down {idx} {note.StartTime} {_timer}");
                var dist = Mathf.Abs(note.StartTime - _timer);
                if (dist < (threshold)) // / note.Duration
                {
                    _accuracy += 1 - (dist / threshold); // dist * note.Duration
                }

                _currentNoteIndexForInput = _currentNoteIndex;
                _holding = true;
            }
            
            if (Input.GetKeyUp(keyCode) && _holding)
            {
                // Debug.Log($"up {idx} {note.StartTime + note.Duration} {_timer}");
                var dist = Mathf.Abs(note.StartTime + note.Duration - _timer);
                if (dist < (threshold)) // / note.Duration
                {
                    _accuracy += 1 - (dist  / threshold); // dist * note.Duration
                }
                
                _finishedIndex = idx;
                NoteEnd(_accuracy / 2);
                
                _accuracy = 0;
                _holding = false;
                _currentNoteIndex = Mathf.Max(_currentNoteIndexForInput + 1, _currentNoteIndex);
            } else if (Input.GetKeyUp(keyCode))
            {
                Debug.Log("NOT HOLDING");
            }
        }

        private void NoteEnd(float accuracy)
        {
            Debug.Log($"accuracy: {accuracy} fin: {_finishedIndex}");
        }
    }
}