using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Button2 : UI_Popup
{
    enum Buttons {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject
    }

    enum Images
    {
        ItemIcon
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
    base.Init();

    Bind<Button>(typeof(Buttons));
    Bind<TextMeshProUGUI>(typeof(Texts));
    Bind<GameObject>(typeof(GameObjects));
    Bind<Image>(typeof(Images));
    GetTextMeshProUGUI((int)Texts.ScoreText).text = "Bind Test2";
    GameObject go = GetImage((int)Images.ItemIcon).gameObject;
    BindEvent(go, (PointerEventData data) => { go.transform.position = data.position;}, Define.UIEvent.Drag);
    
    GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);
    }

    private int _score = 0;

    public void OnButtonClicked(PointerEventData pointerEventData)
    {
    _score++;
    GetTextMeshProUGUI((int)Texts.ScoreText).text = $"점수 : {_score}";
    }
}
