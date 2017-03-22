using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace XXTools
{

    public class Database : MonoBehaviour
    {

        #region   System->PathFrom

        public Name2Path[] name2Paths;

        #endregion

        #region System->FBXSetting
        public ModelImporterAnimationType animType;
        public bool optiomaize;

        public List<GameObject> actorComponesPrefabs;
        #endregion

        public List<GameObject> actorsPrefabs;
        //public List<Actor> actors;

        private Actor[] _actorsCache = null;

        public Actor[] Actors {

            get
            {
                if (_actorsCache!=null)
                {
#if UNITY_EDITOR 
                    if (_actorsCache.Length == actorsPrefabs.Count && (_actorsCache.Length == 0 || (
                        _actorsCache.Length > 0 && _actorsCache[0] != null)))
                        return _actorsCache;
#else
                    return _actorsCache;
#endif
                }
                _actorsCache = new Actor[actorsPrefabs.Count];

                for (int i =0;i< actorsPrefabs.Count;i++)
                {
                    if (actorsPrefabs[i] == null) _actorsCache[i] = null;
                    else _actorsCache[i] = actorsPrefabs[i].GetComponent<Actor>();
                }

                return _actorsCache;
            }

        }


#region Init Data
        public static void InitData(Database db)
        {
            InitPathFrom(db);
            InitFBXSetting(db);
            InitActors(db);
        }

        private static void InitPathFrom(Database db)
        {
            db.name2Paths = new Name2Path[3];

            db.name2Paths[0] = new Name2Path("Model Path", "Assets/Model/");
            db.name2Paths[1] = new Name2Path("Anim Clip Path", "Assets/Anim/");
            db.name2Paths[2] = new Name2Path("Anim Control Path", "Assets/AnimCtrl");

        }

        private static void InitFBXSetting(Database db)
        {
            db.animType = ModelImporterAnimationType.Generic;
            db.optiomaize = true;
            //db.animType = Actor.
        }
        
        private static void InitActors(Database db)
        {
            db.actorsPrefabs = new List<GameObject>();
            
        }

#endregion
        
        [System.Serializable]
        public class Name2Path
        {
            public string name;
            public string path;
            public Name2Path(string name,string path)
            {
                this.name = name;
                this.path = path;
            }

        }
    }
}
