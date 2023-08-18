using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load Prefab : {path}");
            return null;
        }

        return object.Instantiate(prefab);
    }

    public void Destroy(GameObject go, float t =0)
    {
        if (go == null)
            return;
        
        Object.Destroy(go);
    }
}
