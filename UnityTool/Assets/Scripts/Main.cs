using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    public UITableView tableView;
    // Use this for initialization

    void Start()
    {

        GameObject cell = tableView.transform.FindChild("Viewport/Content/Image").gameObject;
        tableView.Init(cell, NumOfCell, null);   
    }


    // Update is called once per frame
    void Update()
    {

    }

    public int NumOfCell()
    {
        return 20;
    }

}
