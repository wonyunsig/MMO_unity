using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Button : MonoBehaviour
{
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, Object[]>();

    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);  //{PointTExt, ScoreText}
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);
        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log($"name : {names[i]}");
            objects[i] = Util.FindChild<T>(gameObject, names[i]);
        }
    }

    private Object FindChild<T>(GameObject go, string name = null, bool recursive = false)
    {
        throw new NotImplementedException();
    }

    private int _score = 0;

    public void OnButtonClicked()
    {
        _score++;
        Debug.Log("ButtonClicked");
        //_text.text = $"점수 : {_score}";
    }
}
