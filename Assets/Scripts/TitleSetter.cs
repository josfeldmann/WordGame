using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSetter : MonoBehaviour
{
    public List<TextMeshProUGUI> textMeshProUGUIs = new List<TextMeshProUGUI>();
    public string title = "Word Game";


    private void Awake() {
        foreach (TextMeshProUGUI t in textMeshProUGUIs) {
            t.SetText(title);
        }
    }
}
