#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CanvasKeyboard {
    [CustomEditor(typeof(CanvasKeyboard))]
    public class CanvasKeyboardEditor : Editor {


        public override void OnInspectorGUI() {
            if (GUILayout.Button("Setup Keys in editor")) {
                ((CanvasKeyboard)target).Setup(true);
            }
            base.OnInspectorGUI();
        }
    }

    
}

#endif


