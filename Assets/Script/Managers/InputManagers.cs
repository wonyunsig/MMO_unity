using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManagers
{
    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
            return;
        
        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
