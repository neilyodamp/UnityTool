using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

    //身体构造部件
    [System.Serializable]
    public class ActorComponent
    {
        public string name;
        public string endString;
        public SkinnedMeshRenderer smr;

        public ActorComponent(string name,string endString,SkinnedMeshRenderer smr)
        {
            this.name = name;
            this.smr = smr;
            this.endString = endString;               
        }

    }

    public enum AnimationType
    {
        Legacy,
        Generic,
    }

    public AnimationType type = AnimationType.Generic;

    private string name = "";
    
    [SerializeField]
    private bool optiomaize = true;

    [SerializeField]
    private List<ActorComponent> components;

    [SerializeField]
    private GameObject majorBody;

    public GameObject MajorBody
    {
        get
        {
            return majorBody;
        }
        //这个set是给编辑器用的
        set
        {
            this.majorBody = value;
        }

    }

    private bool inited = false;
    public Actor(GameObject majorBody)
    {
        this.majorBody = majorBody;

        this.components.Add(new ActorComponent("face", "_face", null));
        this.components.Add(new ActorComponent("body", "_body", null));
        this.components.Add(new ActorComponent("hair", "_hair", null));

    }

    public Actor(GameObject majorBody, List<ActorComponent> components)
    {
        this.majorBody = majorBody;
        this.components = components;
    }

    public void AddComp(ActorComponent component)
    {
        if (component == null)
            return;
        ActorComponent find = null;

        find = this.components.Find(element => element.name.Equals(component.name));
        if ( find!= null)
        {
            find.name = component.name;
            find.smr = component.smr;
            return;
        }

        this.components.Add(component);
        //this.components.
    }
    
    public void RmComp(string name)
    {
        ActorComponent find = null;
        find = this.components.Find(element => element.name.Equals(name));
        if (find != null)
            this.components.Remove(find);
    }

    void Init()
    {
        
        this.inited = true;
        if (components == null) //
            return;

        SkinnedMeshRenderer [] smrs = majorBody.GetComponentsInChildren<SkinnedMeshRenderer>();

        // 创建部件映射信息
        foreach(ActorComponent ac in components)
        {
            for(int i = 0;i < smrs.Length;i++)
            {
                if(smrs[i].gameObject.name.EndsWith(ac.endString))
                {
                    ac.smr = smrs[i];
                    break;
                }
            }        
        }

    }

    void Awake()
    {

        if(!inited)
            Init();

    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
