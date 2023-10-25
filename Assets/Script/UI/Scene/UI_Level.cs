using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Level : UI_Scene
{
    public override void Init()
    {
        Managers.Game.GetPlayer().GetComponent<PlayerStat>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        
    }
}
