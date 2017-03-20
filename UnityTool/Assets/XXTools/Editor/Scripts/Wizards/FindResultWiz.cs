using UnityEngine;
using System.Collections;
using UnityEditor;

namespace XXToolsEditor
{
    public delegate string[] FilterFile(int i);

    public class FindResultWiz : EditorWindow
    {
        private bool inited = false;
        private bool lostFocus = false;

        private Vector2 scroll = Vector2.zero;

        public string[] showList;

       

        static FilterFile filter;
        static int args;

        public static void Show(FilterFile filter,int args)
        {

            FindResultWiz.filter = filter;
            FindResultWiz.args = args;
            FindResultWiz window = EditorWindow.GetWindow<FindResultWiz>(true, "Show Find", true);
     
        }

        private void Init()
        {
            showList = filter(args);
            if(showList == null)
            {
                showList = new string[0];
            }
            inited = true;
#if UNITY_4_5 || UNITY_4_6
            
#else
            titleContent = new GUIContent("Show Find");
#endif
            minSize = new Vector2(300, 450);
            maxSize = new Vector2(300, 450);
        }

        void OnFocus()
        {
            lostFocus = false;
        }
        void OnLostFocus()
        {
            lostFocus = true;
        }

        void Update()
        {
            if (lostFocus) this.Close();
        }

        void OnGUI()
        {
            if(!inited)
            {
                Init();
            }
            XXToolsEdGui.UseSkin();
            GUILayout.Space(10);
            scroll = XXToolsEdGui.BeginScrollView(scroll);
            {
                if (showList == null || showList.Length <= 0)
                {
                    GUILayout.Label("No found", XXToolsEdGui.WarningLabelStyle);
                }
                else
                {
                    foreach(string showItem in showList)
                    {
                        GUILayout.Label(showItem,XXToolsEdGui.CenterLabelStyle);
                    }
                }

            }
            XXToolsEdGui.EndScrollView();
            GUILayout.Space(10);
        }
    }
}
