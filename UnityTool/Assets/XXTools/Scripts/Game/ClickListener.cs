using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler
{
    public delegate void VoidDelegate(GameObject go);


    public static PointerEventData pointEventData;

    public VoidDelegate onClick;


    static public ClickListener Get(GameObject go)
    {
        ClickListener listener = go.GetComponent<ClickListener>();
        if (listener == null)
            listener = go.AddComponent<ClickListener>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onClick != null) onClick(gameObject);
    }
}