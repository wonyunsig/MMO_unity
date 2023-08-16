using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers Instance; //유일성

    public static Managers GetInstance()
    {
        Init();
        return Instance;
    }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject() { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            Instance = go.GetComponent<Managers>();
        }
    }
}
