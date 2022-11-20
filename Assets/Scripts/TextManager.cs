using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextManager : MonoSingleton<TextManager>
{
    public TextMeshProUGUI text;
    public List<string> lines;
    public float shakeDuration = 0.1f, shakePower = 20;
    [SerializeField] private AudioClip carriageReset, bell;
    [SerializeField] private AudioClip[] writeSounds;
    [SerializeField] private AudioSource typewriterSource;
    
    private string currentText = "";
    private int charI=0, textI=0;
    private GameObject obj;

    public void AddText()
    {
        while (lines[textI][charI] == ' ')
        {
            if (charI >= lines[textI].Length) return;
            currentText += lines[textI][charI];
            charI++;
        }
        
        currentText += lines[textI][charI];
        charI++;

        obj.transform.DOShakePosition(shakeDuration,shakePower);
        typewriterSource.PlayOneShotRandomClip(writeSounds);

        if (charI >= lines[textI].Length)
        {
            LineCompleted();
        }

        text.text = currentText;
    }

    private void Start()
    {
        text.text = "";
        obj = text.gameObject;
        baseColor = text.color;

        AlwaysShake();
    }

    void AlwaysShake()
    {
        obj.transform.DOShakePosition(1000,1.7f).OnComplete(AlwaysShake);
    }

    private Color baseColor;
    void LineCompleted()
    {
        typewriterSource.PlayOneShot(bell);
        charI = 0;
        textI++;
        if (textI >= lines.Count) textI = 0;
        currentText = "";

        text.DOColor(Color.yellow, 0.2f);
        obj.transform.DOScaleY(obj.transform.localScale.y + 0.1f, 0.2f);
        obj.transform.DOMoveY(obj.transform.position.y + 1, 0.2f).OnComplete(
            () =>
            {
                text.text = "";
                obj.transform.DOScaleY(obj.transform.localScale.y - 0.1f,0);
                text.DOColor(baseColor, 0f);
                obj.transform.DOMoveY(obj.transform.position.y - 1, 0);
                typewriterSource.PlayOneShot(carriageReset);
            });
        
    }

    public void LineFailed()
    {

        charI = 0;
        textI++;
        if (textI >= lines.Count) textI = 0;
        currentText = "";
        
        text.DOColor(Color.red, 0.2f);
        obj.transform.DOScaleY(obj.transform.localScale.y - 0.1f, 0.2f);
        obj.transform.DOMoveY(obj.transform.position.y - 3, 0.2f).OnComplete(
            () =>
            {
                text.text = "";
                obj.transform.DOScaleY(obj.transform.localScale.y + 0.1f,0);
                text.DOColor(baseColor, 0f);
                obj.transform.DOMoveY(obj.transform.position.y + 3, 0);
            });
    }
}
