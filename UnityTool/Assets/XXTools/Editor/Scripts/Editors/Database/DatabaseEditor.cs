using UnityEngine;
using UnityEditor;
using System.Collections;
using XXTools;

namespace XXToolsEditor
{

    public class DatabaseEditor : EditorWindow
    {
        public Database db = null;
        public static float LeftPanelWidth = 200f;
        private int currDbArea = 0;

        private bool inited = false;

        public static void ShowEditor()
        {

            DatabaseEditor ed = EditorWindow.GetWindow<DatabaseEditor>();
            ed.inited = false;
            ed.db = XXToolsEditorGlobal.DB;
            ed.Show();
        }

        private void Init()
        {
            inited = true;
            titleContent = new GUIContent("XXTools");
            minSize = new Vector2(900, 600);
            wantsMouseMove = true;
        }

        public void ShowErrorMessage(string msg)
        {
            Debug.LogError(msg);
            this.ShowNotification(new GUIContent(msg));
        }

        public void Update()
        {
            if (XXToolsEditorGlobal.DBEditors.Length == 0) return;
            if (currDbArea >= XXToolsEditorGlobal.DBEditors.Length) currDbArea = 0;
            XXToolsEditorGlobal.DBEditors[currDbArea].Update(this);
        }

        void OnGUI()
        {
            if (!inited) Init();

            if(db == null)
            {
                Close();
                return;
            }

            if (XXToolsEditorGlobal.DBEditors.Length == 0) return;
            if (currDbArea >= XXToolsEditorGlobal.DBEditors.Length) currDbArea = 0;
            XXToolsEdGui.UseSkin();

            int prev = currDbArea;
            if(currDbArea != prev)
            {
                XXToolsEditorGlobal.DBEditors[prev].OnDisable(this);
                XXToolsEditorGlobal.DBEditors[currDbArea].OnEnable(this);
                GUI.FocusControl("");
            }

            XXToolsEdGui.DrawHorizontalLine(2f, XXToolsEdGui.ToolbarDividerColor, -3);
            XXToolsEditorGlobal.DBEditors[currDbArea].OnGUI(this);

            if(Event.current.type == EventType.MouseMove)
            {
                Repaint();
            }
        }
    }
}
