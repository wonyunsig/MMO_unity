using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        
        //Get<TextMeshProUGUI>((int)Texts.ScoreText).text = "Bind test";
        GetTextMeshProUGUI((int)Texts.ScoreText).text = "Bind Test2";
    }


    private int _score = 0;
    
    public void OnButtonClicked()
    {
        _score++;
        //_text.text = $"점수 : {_score}";
    }
}