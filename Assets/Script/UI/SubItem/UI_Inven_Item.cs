using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    private string _name;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<TextMeshProUGUI>().text = _name;
        
        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent((PointerEventData data) => { Debug.Log($"아이템 클릭! {_name}");}); }

    private void Start()
    {
        Init();
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
