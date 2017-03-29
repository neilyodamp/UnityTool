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
    public ScrollRect alterCompSV;
    public Button alterButton;
    public Button closeAlterButton;

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
            newButton.transform.SetParent(actorButton.transform.parent);
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
        compButton.gameObject.SetActive(true);
        //compButton.transform.parent.childCount;
        Transform parent = compButton.transform.parent;
       
        for (int j = 1; j < parent.childCount; j++)
        {
            GameObject.Destroy(parent.GetChild(j).gameObject);
        }

        Main.ins.currCompIdx = 0;

        if (Main.ins.currActorGo == null)
            return;

        Actor actor = Main.ins.currActorGo.GetComponent<Actor>();
        var acList = actor.components;
        List<GameObject> buttonList;
        buttonList = new List<GameObject>();

        Vector3 initPos = compButton.transform.localPosition;

        for(int i = 0;i<acList.Count;i++)
        {
            GameObject newButton;
            newButton = Instantiate<GameObject>(compButton.gameObject);
            newButton.transform.SetParent(compButton.transform.parent);
            newButton.name = i.ToString();
            newButton.transform.localPosition = new Vector3(initPos.x, initPos.y - i * 40, initPos.z);
            newButton.transform.Find("Text").GetComponent<Text>().text = acList[i].name;
            ClickListener.Get(newButton).onClick = ClickComp;
        }

        compButton.gameObject.SetActive(false);
    }
    
    void ClickComp(GameObject go)
    {
        int i = int.Parse(go.name);
        Main.ins.currCompIdx = i;
        ShowAlterCompSV(i);
    }


    public void ShowAlterCompSV(int i)
    {
        alterCompSV.gameObject.SetActive(true);
        Actor actor = Main.ins.currActorGo.GetComponent<Actor>();
        List<Actor.ActorComponent> list = actor.GetComponents();
        Actor.ActorComponent ac = list[Main.ins.currCompIdx];
        InitAlterCompSV(ac);
          
    }
    public void HideAlterCompSV()
    {
        alterCompSV.gameObject.SetActive(false);
        
    }

    
    public void InitAlterCompSV(Actor.ActorComponent ac)
    {
        List<string> showList = ac.canSwapList;

        Transform parent = alterButton.transform.parent;

        for (int j = 1; j < parent.childCount; j++)
        {
            GameObject.Destroy(parent.GetChild(j).gameObject);
        }

        List<GameObject> buttonList;
        buttonList = new List<GameObject>();
        alterButton.gameObject.SetActive(true);
        Vector3 initPos = alterButton.transform.localPosition;

        for (int i = 0; i < showList.Count; i++)
        {
            GameObject newButton = Instantiate<GameObject>(alterButton.gameObject);
            newButton.transform.SetParent(alterButton.transform.parent);
            newButton.name = i.ToString();
            newButton.transform.localPosition = new Vector3(initPos.x, initPos.y - i * 40, initPos.z);
            newButton.transform.Find("Text").GetComponent<Text>().text = showList[i];
            ClickListener.Get(newButton).onClick = ClickAlter;
        }

        Transform contentTs = alterButton.transform.parent;
        RectTransform rectTs = contentTs as RectTransform;

        Rect rect = rectTs.rect;
        rect.height = showList.Count * 40 + 10;
        //Debug.Log(rect.width);
        //rectTs.sizeDelta = new Vector2(rect.width, rect.height);
        
        
        alterButton.gameObject.SetActive(false);
    }
    void ClickAlter(GameObject go)
    {
        int i = int.Parse(go.name);
        Actor actor = Main.ins.currActorGo.GetComponent<Actor>();

        Actor.ActorComponent comp = actor.GetComp(Main.ins.currCompIdx);
        actor.gameObject.SetActive(false);
        comp.SwapMesh(i);
        actor.gameObject.SetActive(true);
    }
}
