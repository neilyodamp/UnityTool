using UnityEngine;
using UnityEditor;
using XXTools;
using XXToolsEditor;
using System.Collections;

namespace XXToolsEditor
{
    public class AssetsPostManager : AssetPostprocessor
    {

        //1.处理材质球
        /*
        public Material OnAssigbMaterialModel(Material material,Renderer renderer)
        {

        }
        */
        public void OnPreprocessModel()
        {
            //assetPath
            ModelImporter modelImporter = assetImporter as ModelImporter;
            Debug.Log(assetPath);

            //处理模型导入设置
            if (assetPath.Contains(XXToolsEditorGlobal.DB.name2Paths[0].path))
                PreProcessModel(modelImporter);

        }

        public void OnPostprocessModel(GameObject go)
        {
            //XXToolsEditorGlobal.DB;
            ModelImporter modelImporter = assetImporter as ModelImporter;
            
        }

        private void PreProcessModel(ModelImporter importer)
        {
            importer.animationType = XXToolsEditorGlobal.DB.animType;
            importer.optimizeGameObjects = XXToolsEditorGlobal.DB.optiomaize;


        }
    }
}