using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;  //유일성
    static Managers Instance { get { Init(); return s_Instance; } }

    private InputManagers _input = new InputManagers();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();
    private SceneManagerEx _Scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private PoolManager _pool = new PoolManager();
    private DataManager _data = new DataManager();
    public static InputManagers Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    
    public static UIManager UI { get { return Instance._ui; } }
    
    public static SceneManagerEx Scene { get { return Instance._Scene; } }
    
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }
    

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _input.OnUpdate();
    }

    private static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject() { name = "@Managers"};
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
            s_Instance._sound.Init();
            s_Instance._pool.Init();
            s_Instance._data.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
        
        Pool.Clear();
    }
}