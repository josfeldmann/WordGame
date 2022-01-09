using System.Collections.Generic;
using UnityEngine;

namespace CanvasKeyboard {
    [CreateAssetMenu(menuName = "Keyboard/KeyboardData")]
    public class KeyboardData : ScriptableObject {
        public List<KeyDataRow> keyDataRow = new List<KeyDataRow>();
    }

    
}


