using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using XXTools;

namespace XXToolsEditor
{
    
    public class CreateActorWiz : EditorWindow
    {
        private string filterStr;
        private bool inited;
        private bool lostFocus = false;
        private List<string> showList;
        private List<FileInfo> fileInfoList;
        private Vector2 scroll = new Vector2();
        private int select;

        public static void Show()
        {
            CreateActorWiz window = EditorWindow.GetWindow<CreateActorWiz>(true,"Create Actor",true);
            window.inited = false;

        }

        public void Init()
        {
            inited = true;
            filterStr = "";
            minSize = new Vector2(250,500);
            maxSize = new Vector2(250, 500);
            
            FindShowList();
        }

        private void FindShowList()
        {
            select = 0;
            showList = new List<string>();
            fileInfoList = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo(XXToolsEdUtil.FullProjectPath + XXToolsEditorGlobal.DB.name2Paths[0].path);
            FileInfo[] files = dir.GetFiles("*.fbx", SearchOption.AllDirectories);
            bool filter = false;
            if(filterStr.Length > 0)
            {
                filter = true;
            }
            foreach(FileInfo fileInfo in files)
            {
                if (filter && !fileInfo.Name.Contains(filterStr))
                    continue;

                showList.Add(fileInfo.Name);
                fileInfoList.Add(fileInfo);  
            }
        }
        //private void 
        public void OnGUI()
        {
            if (!inited) Init();
            XXToolsEdGui.UseSkin();
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            string str = EditorGUILayout.TextField(filterStr);
            if(!str.Equals(filterStr)) //关键字改变，重新编列
            {
                filterStr = str;
                FindShowList();
            }
            GUILayout.Space(3);
            XXToolsEdGui.DrawHorizontalLine(1, XXToolsEdGui.DividerColor, 10, 10);
            scroll = XXToolsEdGui.BeginScrollView(scroll);

            //select = GUILayout.Toolbar(select,showList.ToArray(), XXToolsEdGui.ToolbarStyle);
            select = XXToolsEdGui.Menu2(select, showList.ToArray(), GUILayout.Width(250));
            XXToolsEdGui.EndScrollView();
            XXToolsEdGui.DrawHorizontalLine(1, XXToolsEdGui.DividerColor, 10, 10);
            GUILayout.Space(3);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();


                if (GUILayout.Button("Select", XXToolsEdGui.ButtonStyle)) {
                    FileInfo [] fileInfos = fileInfoList.ToArray();

                    CreateActor(fileInfos[select]);

                    this.Close();
                };
                GUI.enabled = true;

                if (GUILayout.Button("Cancel", XXToolsEdGui.ButtonStyle)) {
                    this.Close();
                };
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        
        void OnFocus() {
            if (lostFocus)
            {
                //FindShowList();
            }
            lostFocus = false;    
        }
        void OnLostFocus() {
            lostFocus = true;
        }
        void Update()
        {
            //if (lostFocus) this.Close();
        }
        private void CreateActor(FileInfo fileInfo)
        {
            string path = fileInfo.FullName;
            path = XXToolsEdUtil.ProjectRelativePath(path);
            GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (go == null)
                return;
            CreateActor(go);
            EditorUtility.UnloadUnusedAssetsImmediate();
        }

        private void CreateActor(string [] paths)
        {
            foreach(string path in paths)
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(path,typeof(GameObject)) as GameObject ;
                CreateActor(go);
            }
            EditorUtility.UnloadUnusedAssetsImmediate();
        }

        private void CreateActor(GameObject model)
        {

            string fn = XXToolsEditorGlobal.DB_ACTOR_PATH + model.name + ".prefab";

            if (XXToolsEdUtil.RelativeFileExist(fn))
                fn = AssetDatabase.GenerateUniqueAssetPath(fn);

            GameObject go = PrefabUtility.CreatePrefab(fn, model);

            XXToolsEditorGlobal.DB.actorsPrefabs.Add(go);
            EditorUtility.SetDirty(XXToolsEditorGlobal.DB);
            AssetDatabase.SaveAssets();


            Actor actor = go.AddComponent<Actor>();
            // 扫描 skinnedMesh
            SkinnedMeshRenderer [] smrs =  model.GetComponentsInChildren<SkinnedMeshRenderer>();

            List<Actor.ActorComponent> acList = new List<Actor.ActorComponent>();

            foreach(SkinnedMeshRenderer smr in smrs)
            {
                string[] strs = smr.name.Split('_');
                string endStr =  "_"+strs[strs.Length-1];
                Actor.ActorComponent ac = new Actor.ActorComponent(strs[strs.Length-1], endStr,smr);
                acList.Add(ac);
            }
                     
            actor.InitActor(actor.gameObject, acList);
        }

    }
}
