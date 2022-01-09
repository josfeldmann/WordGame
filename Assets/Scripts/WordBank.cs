using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "WordBank")]
public class WordBank : ScriptableObject
{

    public string wordBankName;
    public TextAsset commonWordText, answerWordText;
    public int wordLength = 5;

}
