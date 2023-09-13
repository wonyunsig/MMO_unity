using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int _order = 10;
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = (_order);
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowPopupUI<T>(string prefabName = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{prefabName}");
        T popup = Util.GetOrAddComponent<T>(go);
        
        _popupStack.Push(popup);

        GameObject root = GameObject.Find("@UI_Root");
        if (root == null)
            root = new GameObject { name = "@UI_Root"};
        go.transform.SetParent(root.transform);
        
        return popup;
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        
        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }
    
    public void ClosePopupUI(UI_Popup pop)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != pop)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
    
}