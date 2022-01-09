using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasKeyboard {
    public class KeyboardKey : MonoBehaviour {

        public KeyData keyData;
        public bool isShifted;
        public CanvasKeyboard keyboard;

        public TextMeshProUGUI keyboardText, altText;

        public void SetShift(bool t) {
            isShifted = t;
            SetKeyData(keyData, keyboard);
        }

        public void SetKeyData(KeyData data, CanvasKeyboard k) {
            keyData = data;
            if (isShifted && (keyData.keyType == KeyType.LETTERCHAR || keyData.keyType == KeyType.SHIFTCHAR)) {
                if (keyboardText) keyboardText.text = data.shiftChar.ToString();
            } else {
                if (keyboardText) keyboardText.text = data.normalChar.ToString();
            }

            if (keyData.keyType == KeyType.SHIFTCHAR) {
                if (altText)altText.gameObject.SetActive(true);
                if (isShifted) {
                    if (altText) altText.SetText(keyData.normalChar.ToString());
                } else {
                    if (altText) altText.SetText(keyData.shiftChar.ToString());
                }
            } else {
                if (altText) altText.gameObject.SetActive(false);
            }
            keyboard = k;
        }

        public void PressKey() {
            char c = keyData.normalChar;

            switch (keyData.keyType) {
                case KeyType.LETTERCHAR:
                    if (isShifted) c = keyData.shiftChar;
                    keyboard.PressKey(c);
                    break;
                case KeyType.SHIFTCHAR:
                    if (isShifted) c = keyData.shiftChar;
                    keyboard.PressKey(c);
                    break;
                case KeyType.SINGULARCHAR:
                    keyboard.PressKey(c);
                    break;
                case KeyType.SPACE:
                    keyboard.PressKey(' ');
                    break;
                case KeyType.SHIFT:
                    keyboard.ShiftToggle();
                    break;
                case KeyType.TAB:
                    keyboard.PressKey('\t');
                    break;
                case KeyType.BACKSPACE:
                    keyboard.BackSpace();
                    break;
                case KeyType.DONE:
                    keyboard.Done();
                    break;
            }
            keyboard.SanitizeBuildString();


        }

        public Image image;

        public void SetNeutralColor() {
            image.color = WordColors.instance.LIGHTGREY;
        }

        public void SetGreen() {
            image.color = WordColors.instance.GREEN;
        }

        public void SetYellow() {
            image.color = WordColors.instance.YELLOW;

        }

        public void SetNone() {
            image.color = WordColors.instance.GREY;
        }

    }


}


