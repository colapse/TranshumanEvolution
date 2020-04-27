using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public RectTransform rectTransform;
    public Text toolTipTextTitle;
    public Text toolTipTextDesc;
    public int maxHorizontalChars = 150;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = FindObjectOfType<RectTransform>();
    }

    public void ShowToolTip()
    {
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }


    public void SetTitle(string text)
    {
        toolTipTextTitle.text = AddBreaks(text);
    }
    
    public void SetDescription(string text)
    {
        toolTipTextDesc.text = AddBreaks(text);
    }

    private string AddBreaks(string text)
    {
        string newText = "";
        for (int i = 0; i < text.Length; i++)
        {
            newText += text[i];
            if (i != 0 && i % maxHorizontalChars == 0) newText += "\n";
        }

        return newText;
    }

}
