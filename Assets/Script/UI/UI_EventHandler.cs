using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnDragHandler = null; 
    public Action<PointerEventData> OnClickHandler { get; set; }


    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
        //transform.position = eventData.position;
        Debug.Log("OnDrag");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
        Debug.Log("OnClick");
    }
}
