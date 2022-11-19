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
        charI = 0;
        textI++;
        if (textI >= lines.Count) textI = 0;

        text.DOColor(Color.yellow, 0.5f);
        obj.transform.DOScaleY(obj.transform.localScale.y + 0.1f, 0.5f);
        obj.transform.DOMoveY(obj.transform.position.y + 1, 0.5f).OnComplete(
            () =>
            {
                currentText = "";
                text.text = "";
                obj.transform.DOScaleY(obj.transform.localScale.y - 0.1f,0);
                text.DOColor(baseColor, 0f);
                obj.transform.DOMoveY(obj.transform.position.y - 1, 0);
            });
        
    }

    public void LineFailed()
    {
        charI = 0;
        currentText = "";
        text.text = "";
        textI++;
        if (textI >= lines.Count) textI = 0;
        
        text.DOColor(Color.red, 1f);
        obj.transform.DOScaleY(obj.transform.localScale.y - 0.1f, 1f);
        obj.transform.DOMoveY(obj.transform.position.y - 3, 1f).OnComplete(
            () =>
            {
                currentText = "";
                text.text = "";
                obj.transform.DOScaleY(obj.transform.localScale.y + 0.1f,0);
                text.DOColor(baseColor, 0f);
                obj.transform.DOMoveY(obj.transform.position.y + 25, 0);
            });
    }
}
