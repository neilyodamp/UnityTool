using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace XXToolsEditor
{
    public class XXToolsEdUtil
    {
        private static string _projectPath;

        public static string FullProjectPath
        {
            get
            {
                if (_projectPath == null)
                {
                    _projectPath = Application.dataPath;
                    _projectPath = _projectPath.Substring(0, _projectPath.LastIndexOf("Assets"));
                }
                return _projectPath;
            }
        }

        public static string FullProjectAssetsPath
        {
            get
            {
                if (_projectAssetsPath == null) _projectAssetsPath = Application.dataPath;
                return _projectAssetsPath;
            }
        }
        private static string _projectAssetsPath;

        public static bool RelativeFileExist(string filePath)
        {
            filePath = FullProjectPath + "/" + filePath;
            return File.Exists(filePath);
        }


        public static string ProjectRelativePath(string fullPath)
        {
            fullPath = fullPath.Replace("\\", "/");

            // ...
            if (fullPath.StartsWith(Application.dataPath))
            {
                return ("Assets" + fullPath.Remove(0, Application.dataPath.Length));
            }
            return null;
        }

        public static bool RelativePathExist(string path)
        {
            path = FullProjectPath + "/" + path;
            return Directory.Exists(path);
        }

        public static List<T> FindPrefabsOfTypeAll<T>(string progressbarTitle, string progressbarInfo)
        where T : Component
        {
            DirectoryInfo dir = new DirectoryInfo(XXToolsEdUtil.FullProjectAssetsPath);
            FileInfo[] files = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
            string fn = "";

            float progress = 0f;
            float step = 1f / (float)files.Length;
            EditorUtility.DisplayProgressBar(progressbarTitle, progressbarInfo, progress);

            List<T> res = new List<T>();
            for (int i = 0; i < files.Length; i++)
            {
                progress += step;
                EditorUtility.DisplayProgressBar(progressbarTitle, progressbarInfo, progress);
                if (files[i] == null) continue;
                fn = XXToolsEdUtil.ProjectRelativePath(files[i].FullName);
                if (!string.IsNullOrEmpty(fn))
                {
                    Object obj = AssetDatabase.LoadAssetAtPath(fn, typeof(T));
                    if (obj != null) res.Add((T)obj);
                }
            }
            EditorUtility.ClearProgressBar();

            return res;
        }

        public static List<string> FindFileNames(string path,string pattern = ".*")
        {
            List<string> findes = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] files = dir.GetFiles(pattern, SearchOption.AllDirectories);

            
            foreach (FileInfo file in files)
            {
                findes.Add(file.Name);
            }
            

            return findes;
        }

        public static List<T> CleanupList<T>(List<T> list)
        where T : UnityEngine.Object
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null || !list[i]) list.RemoveAt(i);
            }

            return list;
        }
    }
}
