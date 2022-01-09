using TMPro;
using UnityEngine;

namespace CanvasKeyboard {
    public class LengthString : MonoBehaviour {
        public TextMeshProUGUI text;
        public string templatefront = "";
        public string templateback = "";
        public CanvasKeyboard canvasKeyboard;

        public void UpdateLength(string s) {
            text.text = templatefront + s.Length.ToString() + "/" + canvasKeyboard.maxBuildStringSize.ToString() + templateback;
        }




    }

    
}


