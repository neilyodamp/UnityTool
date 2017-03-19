using UnityEngine;
using System.Collections;
using UnityEditor;

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
                        
        }

        private void FBXSetting()
        {

        }

        #endregion
    }
}
