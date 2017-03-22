using UnityEngine;
using System.Collections;
using UnityEditor;
using XXTools;
namespace XXToolsEditor
{
    public class TextInputWiz : EditorWindow
    {
        public string text = string.Empty;

        public bool accepted = false;   
        private bool lostFocus = false;

        private XXToolsBasicEventHandler OnAccept = null;
        private XXToolsArgsEventHandler OnAccept2 = null;

        private string label = string.Empty;
        private object[] args = null;

        // ================================================================================================================

        public static void Show(string title, string label, string currText, XXToolsBasicEventHandler onAccept)
        {
            TextInputWiz window = EditorWindow.GetWindow<TextInputWiz>(true, title, true);
            window.inited = false;
            window.text = currText;
            window.label = label;
            window.OnAccept = onAccept;
            window.ShowUtility();
        }

        public static void Show(string title, string label, string currText, XXToolsArgsEventHandler onAccept, object[] args)
        {
            TextInputWiz window = EditorWindow.GetWindow<TextInputWiz>(true, title, true);
            window.inited = false;
            window.text = currText;
            window.label = label;
            window.OnAccept2 = onAccept;
            window.args = args;
            window.ShowUtility();
        }

        private bool inited = false;
        private void Init()
        {
            inited = true;
            minSize = new Vector2(250, 500);
            maxSize = new Vector2(250, 500);
        }

        void OnFocus() { lostFocus = false; }
        void OnLostFocus() { lostFocus = true; }

        void Update()
        {
            if (lostFocus) this.Close();
            if (accepted && OnAccept != null) OnAccept(this);
            if (accepted && OnAccept2 != null) OnAccept2(this, args);
        }

        void OnGUI()
        {
            if (inited) Init();
            XXToolsEdGui.UseSkin();
            EditorGUILayout.Space();

            GUILayout.Label(label);
            text = EditorGUILayout.TextField(text);

            XXToolsEdGui.DrawHorizontalLine(1, XXToolsEdGui.DividerColor, 10, 10);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Accept", XXToolsEdGui.ButtonStyle)) accepted = true;
                if (GUILayout.Button("Cancel", XXToolsEdGui.ButtonStyle)) this.Close();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

    }
}