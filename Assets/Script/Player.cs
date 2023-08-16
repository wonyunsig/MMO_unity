using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers mg = Managers.GetInstance();
        Debug.Log("mg : " + mg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
