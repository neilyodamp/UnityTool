using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace XXToolsEditor
{
    [DatabaseEditor("System",Priority =0)]
    public class SystemEditor : DatabaseEdBase
    {
        private Vector2[] scroll = {Vector2.zero };
        private int selected = 0;

        private static readonly string[] MenuItems = {
            "Path From",
            "FBX Setting",
        };

        public override void OnGUI(DatabaseEditor ed)
        {
            base.OnGUI(ed);
            EditorGUILayout.BeginHorizontal();
            {
                selected = XXToolsEdGui.Menu(selected, MenuItems, GUILayout.Width(180));

                scroll[0] = EditorGUILayout.BeginScrollView(scroll[0]);
                {
                    switch(selected)
                    {
                        case 0: PathFrom(); break;
                        case 1: FBXSetting(); break;
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndScrollView(); //0
            }
            EditorGUILayout.EndHorizontal();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(ed.db);
            }
        }

        #region 子菜单
        private void PathFrom()
        {
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(600));
            {
                GUILayout.Space(15);
                GUILayout.Label("Path From", XXToolsEdGui.Head2Style);

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(XXToolsEdGui.BoxStyle);
                {
                    GUILayout.Label("Avatar Model Path", XXToolsEdGui.Head4Style);

                   
                    for(int i = 0;i<ed.db.name2Paths.Length;i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ed.db.name2Paths[i].path = EditorGUILayout.TextField(new GUIContent(ed.db.name2Paths[i].name), ed.db.name2Paths[i].path);

                        if (GUILayout.Button("Show Find",GUILayout.MaxWidth(100)))
                        {
                            FindResultWiz.Show(ShowFindNamePath,i);
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                                      
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();
        }

        private string [] ShowFindNamePath(int i)
        {

            string pattern = ".*";

            switch (i)
            {
                case 0: pattern = "*.fbx"; break;
                case 1: pattern = "*.anim"; break;
                case 2: pattern = "*.controller"; break;
            }

            string path = XXToolsEdUtil.FullProjectPath + ed.db.name2Paths[i].path;
            Debug.Log("path:"+ path);
            List<string> findes =  XXToolsEdUtil.FindFileNames(path, pattern);
            return findes.ToArray();
        }

        private void FBXSetting()
        {

        }

        #endregion
    }
}
