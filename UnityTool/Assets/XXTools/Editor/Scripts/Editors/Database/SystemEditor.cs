using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace XXToolsEditor
{
    [DatabaseEditor("System",Priority =0)]
    public class SystemEditor : DatabaseEdBase
    {
        private Vector2[] scroll = {Vector2.zero ,Vector2.zero};
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

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(600));
            {
                GUILayout.Space(15);
                GUILayout.Label("Basic Setting", XXToolsEdGui.Head2Style);

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(XXToolsEdGui.BoxStyle,GUILayout.MaxWidth(300));
                {
                    GUILayout.Label("PostProcess", XXToolsEdGui.Head4Style);
                    ModelImporterAnimationType selectType = (ModelImporterAnimationType)EditorGUILayout.EnumPopup("Animation Type", ed.db.animType);
                    if (selectType == ModelImporterAnimationType.Generic || selectType == ModelImporterAnimationType.Legacy)
                    {
                        ed.db.animType = selectType;
                    }
                    if (ed.db.animType == ModelImporterAnimationType.Generic)
                        ed.db.optiomaize = GUILayout.Toggle(ed.db.optiomaize, "Optiomaize");

                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(15);
                GUILayout.Label("Actor Component", XXToolsEdGui.Head2Style);
                EditorGUILayout.BeginVertical(XXToolsEdGui.BoxStyle, GUILayout.MaxWidth(300));
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("AC List", XXToolsEdGui.Head3Style);
                        GUILayout.Space(20);

                        if (XXToolsEdGui.IconButton("Refresh", XXToolsEdGui.Icon_Refresh, GUILayout.Width(100)))
                        {
                            LoadAC();
                        }

                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(20);
                    scroll[1] = XXToolsEdGui.BeginScrollView(scroll[1], XXToolsEdGui.ScrollViewNoTLMarginStyle, GUILayout.Height(275));
                    {
                        
                        foreach (GameObject go in ed.db.actorComponesPrefabs)
                        {
                            EditorGUILayout.SelectableLabel(go.name,GUILayout.Height(30));
                        }
                    }
                    XXToolsEdGui.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private void LoadAC()
        {
            string pathFolder = ed.db.name2Paths[0].path + "Component/";
            DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
            FileInfo[] fileInfos = dirInfo.GetFiles("*.fbx");
            List<GameObject> gos = new List<GameObject>();
            foreach (FileInfo fileInfo in fileInfos)
            {
                string path = XXToolsEdUtil.ProjectRelativePath(fileInfo.FullName);
                GameObject go = AssetDatabase.LoadAssetAtPath(path,typeof(GameObject)) as GameObject;
                string name = go.name;
                go = PrefabUtility.CreatePrefab(XXToolsEditorGlobal.DB_ACTORCOM_PATH + go.name + ".prefab", go);
                gos.Add(go);
            }
            EditorUtility.UnloadUnusedAssetsImmediate();
            ed.db.actorComponesPrefabs = gos;
        }

        #endregion
    }
}
