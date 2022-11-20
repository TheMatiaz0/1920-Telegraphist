using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    public Canvas canvas;
    public TextMeshProUGUI text;
    public SpriteRenderer overlay;
    public List<string> lyrics;
    public Animator anim;

    private int tutorialPhase = -1;
    [HideInInspector]
    public bool isTutorial = false;

    void Next()
    {
        tutorialPhase++;
        if (tutorialPhase >= lyrics.Count)
        {
            isTutorial = false;
            canvas.enabled = false;
            overlay.enabled = false;
            anim.Play("Idle");
            Time.timeScale = 1;
            return;
        }
        text.text = lyrics[tutorialPhase];
        anim.Play("Talk"+tutorialPhase);
    }

    private void Start()
    {
        canvas.enabled = false;
        overlay.enabled = false;
        if (!isTutorial)
        {
            anim.Play("Front");
            Time.timeScale = 0;
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
    }
}
