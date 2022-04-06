using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPageAdapter : MonoBehaviour
{
    public Canvas canvas;
    public int maxDesktopWidth = 1080;
    public RectTransform rect;
    private void Start() {
        if (canvas.GetComponent<RectTransform>().rect.width > maxDesktopWidth) {
            rect.sizeDelta = new Vector2(maxDesktopWidth, canvas.GetComponent<RectTransform>().rect.height);
        } else {
            rect.sizeDelta = new Vector2(canvas.GetComponent<RectTransform>().rect.width, canvas.GetComponent<RectTransform>().rect.height);
        }
    }



}
