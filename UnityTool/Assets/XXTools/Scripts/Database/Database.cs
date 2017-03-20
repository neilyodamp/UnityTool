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
        public Actor.AnimationType animType;
        public bool optiomaize;

        //public Actor.Animatio
        #endregion

        public List<Actor> actors;
        #region Init Data
        public static void InitData(Database db)
        {
            InitPathFrom(db);
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
            db.animType = Actor.AnimationType.Generic;
            db.optiomaize = true;
            //db.animType = Actor.
        }
        
        private static void InitActors(Database db)
        {
            db.actors = new List<Actor>();

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
