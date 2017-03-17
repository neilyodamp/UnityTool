using UnityEngine;
using System.Collections;
using System.IO;

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
            // first convert \ to / cause in unity we want / not \
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

    }
}
