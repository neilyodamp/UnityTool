using UnityEngine;
using System.Collections;
using XXTools;
using UnityEditor;

namespace XXToolsEditor
{
    public class XXToolsEditorMenu
    {
        [MenuItem("XXTools/Open Database", false, 1)]
        public static void OpenDatabaseEditor()
        {
            XXToolsEditorGlobal.OpenDatabaseEditor();
        }
    }
}