using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class UI_System : MonoBehaviour {

    public ScrollRect actorListView;
    public Button actorButton;
    public ScrollRect compScrollView;
    public Button compButton;

    List<GameObject> buttonList;


    // Use this for initialization
    void Start () {
        InitActorListButton();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitActorListButton()
    {
        List<GameObject> actorPrefabs = ResLoad.db.actorsPrefabs;
        List<GameObject> buttonList;
        buttonList = new List<GameObject>();

        Vector3 initPos = actorButton.transform.localPosition;
        
        for (int i = 0; i < actorPrefabs.Count; i++)
        {
            GameObject newButton = Instantiate<GameObject>(actorButton.gameObject);
            newButton.transform.parent = actorButton.transform.parent;
            newButton.name = i.ToString();
            newButton.transform.localPosition = new Vector3(initPos.x,initPos.y- i * 40,initPos.z);
            newButton.transform.Find("Text").GetComponent<Text>().text = actorPrefabs[i].name;
            ClickListener.Get(newButton).onClick = ClickActor;
        }

        Transform contentTs = actorButton.transform.parent;
        RectTransform rectTs = contentTs as RectTransform;
     
        Rect rect = rectTs.rect;
        rect.height = actorPrefabs.Count * 40 + 10;
        rectTs.sizeDelta = new Vector2(rect.width,rect.height);

        actorButton.gameObject.SetActive(false);
    }

    public void ClickActor(GameObject go)
    {
        
        List<GameObject> actorPrefabs = ResLoad.db.actorsPrefabs;
        int idx = int.Parse(go.name);
        if (idx == null) return;

        Main.ins.CreateActor(idx);
        InitCompScrollView();
    }

    void InitCompScrollView()
    {
        Main.ins.currCompIdx = 0;
        /*List<GameObject> actorPrefabs = ResLoad.db.actorsPrefabs;
        List<GameObject> buttonList;
        buttonList = new List<GameObject>();

        Vector3 initPos = actorButton.transform.localPosition;

        for (int i = 0; i < actorPrefabs.Count; i++)
        {
            GameObject newButton = Instantiate<GameObject>(actorButton.gameObject);
            newButton.transform.parent = actorButton.transform.parent;
            newButton.name = i.ToString();
            newButton.transform.localPosition = new Vector3(initPos.x, initPos.y - i * 40, initPos.z);
            newButton.transform.Find("Text").GetComponent<Text>().text = actorPrefabs[i].name;
            ClickListener.Get(newButton).onClick = ClickActor;
        }

        Transform contentTs = actorButton.transform.parent;
        RectTransform rectTs = contentTs as RectTransform;

        Rect rect = rectTs.rect;
        rect.height = actorPrefabs.Count * 40 + 10;
        rectTs.sizeDelta = new Vector2(rect.width, rect.height);

        actorButton.gameObject.SetActive(false);
        */
        if (Main.ins.currActorGo == null)
            return;
        Actor actor = Main.ins.currActorGo.GetComponent<Actor>();
        var acList = actor.components;
        List<GameObject> buttonList;
        buttonList = new List<GameObject>();

        Vector3 initPos = compButton.transform.localPosition;

        for(int i = 0;i<acList.Count;i++)
        {
            GameObject newButton = Instantiate<GameObject>(compButton.gameObject);
            newButton.transform.parent = compButton.transform.parent;
            newButton.name = i.ToString();
            newButton.transform.localPosition = new Vector3(initPos.x, initPos.y - i * 40, initPos.z);
            newButton.transform.Find("Text").GetComponent<Text>().text = acList[i].name;
            ClickListener.Get(newButton).onClick = ClickComp;
        }
    }
    
    void ClickComp(GameObject go)
    {
        int i = int.Parse(go.name);
       
    }

}
