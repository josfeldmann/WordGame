using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasKeyboard {
    public class KeyboardRow : MonoBehaviour {

        public KeyboardKey keyPrefab, shiftKeyPrefab, spaceKeyPrefab, tabKeyPrefab, backSpacePrefab;
        public List<KeyboardKey> keys = new List<KeyboardKey>();
        private CanvasKeyboard keyboard;
  


        public void SetKeyboardRow(KeyDataRow row, CanvasKeyboard keyboard) {
            this.keyboard = keyboard;
            foreach (KeyData key in row.keyData) {
                SpawnKey(key);
            }
        }


        public KeyboardKey SpawnKey(KeyData data) {
            KeyboardKey kPrefab = keyPrefab;
            if (data.keyType == KeyType.SHIFT) {
                kPrefab = shiftKeyPrefab;
            } else if (data.keyType == KeyType.SPACE) {
                kPrefab = spaceKeyPrefab;
            } else if (data.keyType == KeyType.TAB) {
                kPrefab = tabKeyPrefab;
            } else if (data.keyType == KeyType.BACKSPACE) {
                kPrefab = backSpacePrefab;
            }


            KeyboardKey k = Instantiate(kPrefab, transform);
            k.SetKeyData(data, keyboard);
            keys.Add(k);
            return k;
        }



    }

    public class LengthDisplayText : MonoBehaviour {

    }
}


