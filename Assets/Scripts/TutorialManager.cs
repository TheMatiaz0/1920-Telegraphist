using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    public Canvas canvas;
    public TextMeshProUGUI text;
    public SpriteRenderer overlay;
    public List<string> lyrics;
    public List<string> lyricsEn;
    public Animator anim;
    public bool english = true;
    [SerializeField] private GameObject blazeParticle;

    private int tutorialPhase = -1;
    [HideInInspector]
    public bool isTutorial = false;

    void Next()
    {
        tutorialPhase++;
        if (!english)
        {
            if (tutorialPhase >= lyrics.Count)
            {
                isTutorial = false;
                canvas.enabled = false;
                overlay.enabled = false;
                anim.Play("Idle");
                Time.timeScale = 1;
                blazeParticle.SetActive(true);
                SetInputActive(true);
                // CO DO KURWY CZEMU TU S¥ ZDUPLIKOWANE LINIJKI
                return;
            }

            text.text = lyrics[tutorialPhase];
        }
        else
        {
            if (tutorialPhase >= lyricsEn.Count)
            {
                isTutorial = false;
                canvas.enabled = false;
                overlay.enabled = false;
                anim.Play("Idle");
                Time.timeScale = 1;
                blazeParticle.SetActive(true);
                SetInputActive(true);
                // CO DO KURWY CZEMU TU S¥ ZDUPLIKOWANE LINIJKI
                return;
            }

            text.text = lyricsEn[tutorialPhase];
        }

        anim.Play("Talk"+tutorialPhase);
    }

    private void Start()
    {
        StartTutorial();
    }

    private void SetInputActive(bool isActive)
    {
        for (int i = 0; i < Tracks.TrackManager.Current.TrackComponents.Length; i++)
        {
            Tracks.TrackManager.Current.TrackComponents[i].IsInputEnabled = false;
        }
        RadioCirclesController.Current.IsInputEnabled = false;
        TowerController.Current.IsInputEnabled = false;
    }

    private void StartTutorial()
    {
        SetInputActive(false);
        Time.timeScale = 0;
        blazeParticle.SetActive(false);
        canvas.enabled = false;
        overlay.enabled = false;

        if (!isTutorial)
        {
            Next();
            isTutorial = true;
            canvas.enabled = true;
            overlay.enabled = true;
        }
    }

    private void Update()
    {
        if (isTutorial && Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
    }
}
