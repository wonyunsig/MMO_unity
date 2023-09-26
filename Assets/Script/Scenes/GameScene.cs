using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inven>();

        Stat levelStat = Managers.Data.StatDic[1];
        Debug.Log("levelStat.level : " + levelStat.level);
    }

    public override void Clear()
    {
        
    }
}