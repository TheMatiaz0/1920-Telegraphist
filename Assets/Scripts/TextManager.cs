using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<string> lines;
    
    private string currentText = "";
    private int charI=0, textI=0;

    public void AddText()
    {
        currentText += lines[textI][charI];
        charI++;
        if (charI >= lines[textI].Length)
        {
            LineCompleted();
        }

        text.text = currentText;
    }

    void LineCompleted()
    {
        charI = 0;
        currentText = "";
        textI++;
        if (textI >= lines.Count) textI = 0;
    }
}
