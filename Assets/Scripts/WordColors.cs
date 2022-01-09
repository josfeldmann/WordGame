using UnityEngine;

public class WordColors : MonoBehaviour {

    public static WordColors instance;


    private void Awake() {
        instance = this;
    }

    public Color BLACK, GREY, YELLOW, WHITE, GREEN, LIGHTGREY;


}