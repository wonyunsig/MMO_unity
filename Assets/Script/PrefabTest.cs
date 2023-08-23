using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    public float DestroyTime = 3.0f;
    void Start()
    {
        GameObject tank = Managers.Resource.Instantiate("Tank");
        Managers.Resource.Destroy(tank, DestroyTime);
    }
}