using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void VoidDelegate(GameObject go);

    public static PointerEventData pointEventData;

    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onUp;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onDrag;
    public VoidDelegate onEndDrag;
    public object parameter;
    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null)
            listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onClick != null) onClick(gameObject);
        //        if (!UIEventListener.pointEventData.dragging)
        //        {
        //            AudioManager.Ins.Play("buttonClick");
        //        }

        eventData.Reset();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onDown != null) onDown(gameObject);
        eventData.Reset();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onUp != null) onUp(gameObject);
        eventData.Reset();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onEnter != null) onEnter(gameObject);
        eventData.Reset();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onExit != null) onExit(gameObject);
        eventData.Reset();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onBeginDrag != null) onBeginDrag(gameObject);
        //eventData.Reset();
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onDrag != null) onDrag(gameObject);
        eventData.Reset();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onEndDrag != null) onEndDrag(gameObject);
        eventData.Reset();
    }
}