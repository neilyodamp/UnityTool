using UnityEngine;
using System.Collections;
using UnityEditor;
using XXTools;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Reflection;

namespace XXToolsEditor
{
    [InitializeOnLoad]
    public class XXToolsEditorGlobal
    {

        public const string DB_FILE = "Assets/XXTools Data/Database.prefab";
        public const string DB_DATA_PATH = "Assets/XXTools Data/Database_Data/";
        public const string DB_PATH = "Assets/XXTools Data/";

        private static Database _db = null;

        public static string[] DBEdNames { get; private set; }
        public static DatabaseEdBase [] DBEditors { get; private set; }

        public static Database DB
        {
            get
            {
                if (_db == null)
                {

                }
                return _db;
            }
        }
        #region startup/loading & editor callback

        static XXToolsEditorGlobal()
        {
            Assembly[] asms = System.AppDomain.CurrentDomain.GetAssemblies();
            //LoadXXXEditors()
            LoadDatabaseEditors(asms);

            EditorApplication.update += XXToolsEditorGlobal.OnUpdate;

        }

        private static void OnUpdate()
        {
                
        }

        private static void LoadDatabaseEditors(Assembly[] asms)
        {
            List<System.Type> foundEdTypes = new List<System.Type>();

            float progress = 0f;
            float step = 1f;

            for (int i = 0; i < asms.Length; i++)
            {
                progress += step;
                
                System.Type[] types = asms[i].GetExportedTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].IsClass && typeof(DatabaseEdBase).IsAssignableFrom(types[j]) && types[j].Name != "DatabaseEdBase")
                    {
                        foundEdTypes.Add(types[j]);
                    }
                }

            }

            List<XXToolsDBEdInfo> eds = new List<XXToolsDBEdInfo>();

            progress = 0f;
            step = 1f/(float)foundEdTypes.Count;

            for (int i = 0; i < foundEdTypes.Count; i++)
            {
                progress += step;

                DatabaseEditorAttribute att = null;
                System.Object[] attribs = foundEdTypes[i].GetCustomAttributes(typeof(DatabaseEditorAttribute), false);
                if (attribs.Length > 0)
                {
                    for (int j = 0; j < attribs.Length; j++)
                    {
                        att = (attribs[j] as DatabaseEditorAttribute);
                        if (att != null) break;
                    }
                }

                if (att != null)
                {   // now create the editor instance
                    XXToolsDBEdInfo nfo = new XXToolsDBEdInfo();
                    nfo.priority = att.Priority;
                    nfo.name = att.Name;
                    nfo.editor = (DatabaseEdBase)System.Activator.CreateInstance(foundEdTypes[i]);
                    eds.Add(nfo);
                }
                else
                {
                    Debug.LogError("Invalid Database Editor [" + foundEdTypes[i].ToString() + "] encountered.");
                }
            }

            eds.Sort(delegate (XXToolsDBEdInfo a, XXToolsDBEdInfo b) { return a.priority.CompareTo(b.priority); });
            DBEditors = new DatabaseEdBase[eds.Count];
            DBEdNames = new string[eds.Count];
            for (int i = 0; i < eds.Count; i++)
            {
                DBEdNames[i] = eds[i].name;
                DBEditors[i] = eds[i].editor;
            }

        }

        #endregion

        #region init & defaults
        public static bool LoadDatabase(bool silent = false)
        {
            if (XXToolsEditorGlobal._db != null)
                return true;

            if (!XXToolsEdUtil.RelativeFileExist(DB_FILE))
            {
                Debug.LogError("You must first create a Database. From the menu, select: Window -> XXTools -> Database");
                if (!silent) EditorUtility.DisplayDialog("Warning", "You must first create a Database. From the menu, select: Window -> XXTools -> Database", "Close");
                return false;
            }
            else
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(DB_FILE, typeof(GameObject)) as GameObject;
                XXToolsEditorGlobal._db = go.GetComponent<Database>();
                if (_db != null)
                {
                    PerformAfterDBLoaded(_db);
                    return true;
                }

            }
            return false;
        }
        private static void PerformAfterDBLoaded(Database db)
        {

            InitLoadSaveProvider(db, -1);

            UpdateAutoCallList(db);


        }

        public static void InitLoadSaveProvider(Database db, int forceUsing)
        {

        }
        private static void UpdateAutoCallList(Database db)
        {

        }
        public static void LoadOrCreateDatabase()
        {
            if (!XXToolsEdUtil.RelativeFileExist(DB_FILE))
            {
                CreateDatabase();
            }
            else
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(DB_FILE, typeof(GameObject)) as GameObject;
                _db = go.GetComponent<Database>();
                PerformAfterDBLoaded(_db);
            }
        }

        private static void CreateDatabase()
        {
            //CheckTags();

            if (!XXToolsEditorGlobal.SaveCurrentSceneIfUserWantsTo()) return;

            Debug.Log("Creating Database");

            DeleteOldAssets();

            CheckDatabasePaths();

           // CopySystemScenes();

            Object prefab = PrefabUtility.CreateEmptyPrefab(DB_FILE);
            GameObject go = new GameObject("Database"); 
            go.AddComponent<Database>();                
            GameObject dbPrefab = PrefabUtility.ReplacePrefab(go, prefab); 
            GameObject.DestroyImmediate(go);            

            Database db = dbPrefab.GetComponent<Database>();
            XXToolsEditorGlobal.InitDatabaseDefaults(db);

            PerformAfterDBLoaded(db);

            AssetDatabase.Refresh();

        }
        public static void CheckDatabasePath(string parentPath, string newPath)
        {
            if (!XXToolsEdUtil.RelativePathExist(newPath))
            {
                //Debug.Log("Creating: " + newPath);
                parentPath = parentPath.Substring(0, parentPath.LastIndexOf('/'));  // remove last '/'
                newPath = newPath.Substring(0, newPath.LastIndexOf('/'));           // remove last '/'
                newPath = newPath.Substring(newPath.LastIndexOf('/') + 1);          
                AssetDatabase.CreateFolder(parentPath, newPath);
            }
        }

        private static void CheckDatabasePaths()
        {
            CheckDatabasePath("Assets/", DB_PATH);

            //CheckDatabasePath(DB_PATH, DB_SCENE_PATH);
            //CheckDatabasePath(DB_SCENE_PATH, DB_SYS_SCENE_PATH);

            CheckDatabasePath(DB_PATH, DB_DATA_PATH);

            //CheckDatabasePath(DB_DATA_PATH, DB_GUI_PATH);
            //CheckDatabasePath(DB_DATA_PATH, DB_DEFS_PATH);
            //CheckDatabasePath(DB_DATA_PATH, DB_SKILLS_PATH);
            //CheckDatabasePath(DB_DATA_PATH, DB_EVENTS_PATH);
            //CheckDatabasePath(DB_DATA_PATH, DB_CAMERAS_PATH);
            AssetDatabase.Refresh();
        }

        private static void DeleteDatabasePath(string path)
	    {
		    if (XXToolsEdUtil.RelativePathExist(path))
		    {
			    Debug.Log("Deleting all assets in: " + path);
			    path = path.Substring(0, path.LastIndexOf('/'));	// remove last '/'
			    AssetDatabase.DeleteAsset(path);
		    }
	    }
        private static void DeleteOldAssets()
        {
            DeleteDatabasePath(DB_DATA_PATH);

            DeleteDatabasePath(DB_FILE);

            AssetDatabase.Refresh();
        }

        private static void InitDatabaseDefaults(Database db)
        {

        }

        #endregion

        #region menus
        public static void OpenDatabaseEditor()
        {
            LoadOrCreateDatabase();
            DatabaseEditor.ShowEditor();
        }
        #endregion

        #region pub
        public static bool SaveCurrentSceneIfUserWantsTo()
        {
#if UNITY_4_6 || UNITY_5_2
		return EditorApplication.SaveCurrentSceneIfUserWantsTo();
#else
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            return true;
#endif
        }
        #endregion
    }
}
