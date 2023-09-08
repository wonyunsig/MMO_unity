using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Button : UI_Base
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
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));        
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        
        //Get<TextMeshProUGUI>((int)Texts.ScoreText).text = "Bind test";
        GetTextMeshProUGUI((int)Texts.ScoreText).text = "Bind Test2";

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; });
        
        GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);
    }

    private int _score = 0;
    
    public void OnButtonClicked(PointerEventData obj)
    {
        _score++;
        GetTextMeshProUGUI((int)Texts.ScoreText).text = $"점수 : {_score}";
        //_text.text = $"점수 : {_score}";
    }
}