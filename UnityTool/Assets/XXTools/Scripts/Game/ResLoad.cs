using UnityEngine;
using System.Collections;
using UnityEditor;
using XXTools;

public class ResLoad {

    /*
    public static string acPath = "Assets/XXTools Data/Database_Data/ActorsCom/";
    public static string actorPath = "Assets/XXTools Data/Database_Data/Actors/";

    public static GameObject LoadAC(string name)
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(acPath + name) as GameObject;
        return go;
    }

    public static GameObject InstanceActor(Actor actor)
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(acPath + actor.name) as GameObject;
        return null;
    }
    */
    public static Database db;

    public static void Init()
    {
        GameObject dbGo = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/XXTools Data/Database.prefab") as GameObject;
        GameObject go = GameObject.Instantiate<GameObject>(dbGo);
        db = go.GetComponent<Database>();
        //Debug.Log(db.actorsPrefabs.Count);
    }

}
