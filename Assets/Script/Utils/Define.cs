using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }
    
    public enum MouseEvent
    {
        Press,
        Click
    }
    public enum CameraMode
    {
        QuaterView
    }
    
    public enum UIEvent
    {
        Click,
        Drag
    }
    
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }
}
