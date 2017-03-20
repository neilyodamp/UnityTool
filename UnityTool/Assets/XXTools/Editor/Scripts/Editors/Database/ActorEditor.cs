using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace XXToolsEditor
{
    [DatabaseEditor("Actor", Priority = 1)]
    public class ActorEditor : DatabaseEdBase
    {
        private Vector2[] scroll = { Vector2.zero };

        public override void OnGUI(DatabaseEditor ed)
        {
            base.OnGUI(ed);
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
                    TextInputWiz.Show("New Actor", "Enter name for new actor", "", CreateSkill);
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        private void CreateSkill(System.Object sender)
        {
            TextInputWiz wiz = sender as TextInputWiz;
            string name = wiz.text;
            wiz.Close();

            if (string.IsNullOrEmpty(name)) name = "Actor";

            XXToolsEditorGlobal.CheckDatabasePath(XXToolsEditorGlobal.DB_DATA_PATH, XXToolsEditorGlobal.DB_ACTOR_PATH);
            string fn = XXToolsEditorGlobal.DB_ACTOR_PATH + name + ".prefab";
            if (XXToolsEdUtil.RelativeFileExist(fn)) fn = AssetDatabase.GenerateUniqueAssetPath(fn);
            /*

            Object prefab = PrefabUtility.CreateEmptyPrefab(fn);

            //GameObject go = new GameObject(name);                           // create temp object in scene 
            //go.AddComponent<RPGSkill>();

            GameObject toRef = PrefabUtility.ReplacePrefab(go, prefab);     // save prefab
            GameObject.DestroyImmediate(go);                                // clear temp object from scene

            curr = toRef.GetComponent<RPGSkill>();
            curr.screenName = name;
            ed.db.skillPrefabs.Add(toRef);

            EditorUtility.SetDirty(curr);
            
            */

            EditorUtility.SetDirty(ed.db);
            AssetDatabase.SaveAssets();

            ed.Repaint();
        }

    }
}
