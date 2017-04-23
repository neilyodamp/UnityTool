using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RectTransform ts = transform as RectTransform;
        Debug.Log(ts.rect);
        Debug.Log(ts.localPosition);
        Debug.Log(ts.sizeDelta);
	}
	

	// Update is called once per frame
	void Update () {
	
	}

}
