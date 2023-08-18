using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    //public GameObject prefab;
    
    void Start()
    {
        //GameObject prefab = Resources.Load<GameObject>("Prefabs/Tank");
        //GameObject tank = Instantiate(prefab);
        GameObject tank = Managers.Resource.Instantiate("Tank");
        
        //Destroy(tank, 3.0f);
        Managers.Resource.Destroy(tank, 3.0f);
    }

    void Update()
    {
        
    }
}
