using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {


    public List<GameObject> actorList;
    public GameObject currActorGo;
    public int currCompIdx;
    public static Main ins;

    void Awake()
    {
        ins = GetComponent<Main>();
        ResLoad.Init();
    }
    void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateActor()
    {
        CreateActor(0);
    }
    public void CreateActor( int idx )
    {
        List<GameObject> actorPrefabs = ResLoad.db.actorsPrefabs;
        if (idx == null) return;

        GameObject actorGo = actorPrefabs[idx];
        if (actorGo == null)
            return;
        if (currActorGo != null)
        {
            DestroyImmediate(currActorGo);
        }
        currActorGo = Instantiate<GameObject>(actorGo);
        currActorGo.name = actorGo.name;

        currActorGo.transform.localPosition = Vector3.zero;

    }
}
