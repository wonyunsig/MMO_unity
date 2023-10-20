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

        Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;
        //Stat stat = dic[0]; // Key not Found Exception
        Data.Stat stat = dic[1];
        //Debug.Log("levelStat.level : " + levelStat.level);
        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
    }

    public override void Clear()
    {
        
    }
}