using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace XXToolsEditor
{
    [DatabaseEditor("Actor", Priority = 1)]
    public class ActorEditor : DatabaseEdBase
    {
        private Vector2[] scroll = { Vector2.zero ,Vector2.zero};

        private Actor curr = null;
        private Actor del = null; 

        

        public override void OnGUI(DatabaseEditor ed)
        {
            base.OnGUI(ed);
            EditorGUILayout.BeginHorizontal();
            {
                LeftPanel();
                XXToolsEdGui.DrawVerticalLine(2f, XXToolsEdGui.DividerColor, 0f, 10f);
                RightPanel();
            }
            EditorGUILayout.EndHorizontal();
        }


        private void LeftPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(DatabaseEditor.LeftPanelWidth));
            GUILayout.Space(5);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(new GUIContent("Add Actors", XXToolsEdGui.Icon_Plus), EditorStyles.miniButtonLeft))
                {
                    GUI.FocusControl("");
                    //EditorUtility.OpenFilePanel("Select Scene", XXToolsEdUtil.FullProjectAssetsPath, "unity");
                    //TextInputWiz.Show("New Actor", "Enter name for new actor", "", CreateActor);
                    CreateActorWiz.Show();
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            scroll[0] = XXToolsEdGui.BeginScrollView(scroll[0], GUILayout.Width(DatabaseEditor.LeftPanelWidth));
            {
                if(ed.db.Actors.Length > 0)
                {
                    foreach(Actor actor in ed.db.Actors)
                    {
                        if(actor == null)
                        {
                            ed.db.actorsPrefabs = XXToolsEdUtil.CleanupList<GameObject>(ed.db.actorsPrefabs);
                            EditorUtility.SetDirty(ed.db);
                            AssetDatabase.SaveAssets();
                            GUIUtility.ExitGUI();
                            return;
                        }

                        EditorGUILayout.BeginHorizontal(GUILayout.Width(DatabaseEditor.LeftPanelWidth - 20), GUILayout.ExpandWidth(false));
                        {
                            if (XXToolsEdGui.ToggleButton(curr == actor, actor.name, XXToolsEdGui.ButtonLeftStyle, GUILayout.Width(160), GUILayout.ExpandWidth(false)))
                            {
                                GUI.FocusControl("");
                                curr = actor;
                                currAc = null;
                                //GetCurrentEd();
                            }

                            //if (ed.db.Cameras.Length == 1) GUI.enabled = false; // can't allow deleting the camera if there is only one left since runtime depends on at least one being present
                            if (GUILayout.Button("X", XXToolsEdGui.ButtonRightStyle, GUILayout.Width(20))) del = actor;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }else
                {
                    GUILayout.Label("No Actors are defined.", XXToolsEdGui.WarningLabelStyle);
                }                               
            }
            XXToolsEdGui.EndScrollView();

            GUILayout.Space(3);
            EditorGUILayout.EndVertical();

            if(del != null)
            {
                if (curr == del) curr = null;
                ed.db.actorsPrefabs.Remove(del.gameObject);
                EditorUtility.SetDirty(ed.db);
                AssetDatabase.SaveAssets();

                string path = AssetDatabase.GetAssetPath(del.gameObject);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                del = null;
            }
        }

        
        public override void OnEnable(DatabaseEditor ed)
        {
            base.OnEnable(ed);

            int cnt = ed.db.actorsPrefabs.Count;
            ed.db.actorsPrefabs = XXToolsEdUtil.CleanupList<GameObject>(ed.db.actorsPrefabs);
            if(cnt != ed.db.actorsPrefabs.Count)
            {
                EditorUtility.SetDirty(ed.db);
                AssetDatabase.SaveAssets();
            }
        }

        private void GetCurrentEd()
        {
            
        }



        private void RightPanel()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.Label("Actor Definition", XXToolsEdGui.Head1Style);
            if (curr == null) { EditorGUILayout.EndVertical(); return; }
            GUILayout.BeginHorizontal();
            BasicInfo();
            if (currAc == null) { EditorGUILayout.EndHorizontal(); EditorGUILayout.EndVertical(); return; }
            ComInfo();
            GUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }
        private Actor.ActorComponent currAc = null;
        private Actor.ActorComponent delAc = null;
        private  void BasicInfo()
        {
            EditorGUILayout.BeginVertical(XXToolsEdGui.BoxStyle, GUILayout.Width(240));
            //curr.name = EditorGUILayout.TextField("Prefabs Name", curr.name);
            GUILayout.Label(curr.name, XXToolsEdGui.Head3Style);
            GUILayout.Space(10);
            GUILayout.Label("Actor Comp", XXToolsEdGui.Head4Style);

            XXToolsEdGui.BeginScrollView(scroll[1],GUILayout.MaxHeight(200));
            {
                for(int i = 0;i<curr.components.Count;i++)
                {
                    Actor.ActorComponent ac = curr.components[i];
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(DatabaseEditor.LeftPanelWidth - 20), GUILayout.ExpandWidth(false));
                    {

                        if (XXToolsEdGui.ToggleButton(currAc == ac,ac.name, XXToolsEdGui.ButtonLeftStyle, GUILayout.Width(160), GUILayout.ExpandWidth(false)))
                        {
                            GUI.FocusControl("");
                            currAc = ac;
                        }
                        if (GUILayout.Button("X", XXToolsEdGui.ButtonRightStyle, GUILayout.Width(20))) delAc = ac;

                    }
                    EditorGUILayout.EndHorizontal();
                }      
            }
            XXToolsEdGui.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        private void ComInfo()
        {
            EditorGUILayout.BeginVertical(XXToolsEdGui.BoxStyle, GUILayout.Width(300));
            GUILayout.Label(currAc.name, XXToolsEdGui.Head3Style);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Can Swap", XXToolsEdGui.Head4Style);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Add", XXToolsEdGui.Icon_Plus), EditorStyles.miniButtonLeft))
            {
                GUI.FocusControl("");
                MutiSelectWiz.Show();
            }
            
            EditorGUILayout.EndHorizontal();
            XXToolsEdGui.DrawHorizontalLine(1f, XXToolsEdGui.DividerColor);
            GUILayout.Space(7);
            XXToolsEdGui.BeginScrollView(scroll[1], GUILayout.MaxHeight(200));

            XXToolsEdGui.EndScrollView();
            EditorGUILayout.EndVertical();
        }

    }
}
