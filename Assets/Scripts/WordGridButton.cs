using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum WORDBUTTONSTATE { GREEN = 2, YELLOW = 1, EMPTY = 0}

public class WordGridButton : MonoBehaviour {
    public Image backImage, borderImage;
    public TextMeshProUGUI text;
    public char selectedChar;
    public WORDBUTTONSTATE state = WORDBUTTONSTATE.EMPTY;

    public void SetEmpty() {
        text.SetText("");
        backImage.color = WordColors.instance.BLACK;
        borderImage.color = WordColors.instance.GREY;
        state = WORDBUTTONSTATE.EMPTY;
    }

    internal void SetTypedLetter(char v) {
        text.SetText(v.ToString());
        backImage.color = WordColors.instance.BLACK;
        borderImage.color = WordColors.instance.GREY;
        state = WORDBUTTONSTATE.EMPTY;
    }

    internal void SetCorrect(char v) {
        text.SetText(v.ToString());
        backImage.color = WordColors.instance.GREEN;
        borderImage.color = WordColors.instance.GREEN;
        state = WORDBUTTONSTATE.GREEN;
    }

    internal void SetSemiCorrect(char v) {
        text.SetText(v.ToString());
        backImage.color = WordColors.instance.YELLOW;
        borderImage.color = WordColors.instance.YELLOW;
        state = WORDBUTTONSTATE.YELLOW ;
    }

    internal void SetMissing(char v) {
        text.SetText(v.ToString());
        backImage.color = WordColors.instance.BLACK;
        borderImage.color = WordColors.instance.GREY;
        state = WORDBUTTONSTATE.EMPTY;
    }
}
