using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using XXTools;

namespace XXToolsEditor
{
    public class MutiSelectWiz : EditorWindow
    {
        private string filterStr;
        private bool inited;
        private bool lostFocus = false;
        private List<string> showList;
        private List<FileInfo> fileInfoList;

        private Vector2 scroll = new Vector2();
        //private int select;
        private bool[] selecteds;
        public Actor.ActorComponent ac;

        public static void Show(Actor.ActorComponent ac)
        {
            MutiSelectWiz window = EditorWindow.GetWindow<MutiSelectWiz>(true, "Add AC", true);
            window.inited = false;
            window.ac = ac;
        }

        public void Init()
        {
            inited = true;
            filterStr = "";
            minSize = new Vector2(250, 500);
            maxSize = new Vector2(250, 500);

            FindShowList();
        }

        private void FindShowList()
        {
            showList = new List<string>();
            fileInfoList = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo(XXToolsEdUtil.FullProjectPath + XXToolsEditorGlobal.DB_ACTORCOM_PATH);
            FileInfo[] files = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
            bool filter = false;
            if (filterStr.Length > 0)
            {
                filter = true;
            }
            foreach (FileInfo fileInfo in files)
            {
                if (filter && !fileInfo.Name.Contains(filterStr))
                    continue;

                showList.Add(fileInfo.Name);
                fileInfoList.Add(fileInfo);
            }
            selecteds = new bool[fileInfoList.Count];
        }
        //private void 
        public void OnGUI()
        {

            if (!inited) Init();
            XXToolsEdGui.UseSkin();
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            string str = EditorGUILayout.TextField(filterStr);
            if (!str.Equals(filterStr)) //关键字改变，重新编列
            {
                filterStr = str;
                FindShowList();
            }
            GUILayout.Space(3);
            XXToolsEdGui.DrawHorizontalLine(1, XXToolsEdGui.DividerColor, 10, 10);
            scroll = XXToolsEdGui.BeginScrollView(scroll);

            //select = GUILayout.Toolbar(select,showList.ToArray(), XXToolsEdGui.ToolbarStyle);
            selecteds = XXToolsEdGui.MenuMutiSel(selecteds, showList.ToArray(), GUILayout.Width(250));

            XXToolsEdGui.EndScrollView();
            XXToolsEdGui.DrawHorizontalLine(1, XXToolsEdGui.DividerColor, 10, 10);
            GUILayout.Space(3);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();


                if (GUILayout.Button("Select", XXToolsEdGui.ButtonStyle))
                {
                    FileInfo[] fileInfos = fileInfoList.ToArray();

                    //CreateActor(fileInfos[select]);
                    Select();
                    this.Close();
                };
                GUI.enabled = true;

                if (GUILayout.Button("Cancel", XXToolsEdGui.ButtonStyle))
                {
                    this.Close();
                };
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

        }

        void OnFocus()
        {
            if (lostFocus)
            {
                //FindShowList();
            }
            lostFocus = false;
        }
        void OnLostFocus()
        {
            lostFocus = true;
        }
        void Update()
        {
            //if (lostFocus) this.Close();
        }

        private void Select()
        {
            for (int i = 0; i < selecteds.Length; i++)
            {
                if (selecteds[i])
                {
                    string name = fileInfoList[i].Name;
                    name = name.Split('.')[0];
                    if (!ac.canSwapList.Contains(name))
                        ac.canSwapList.Add(name);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
