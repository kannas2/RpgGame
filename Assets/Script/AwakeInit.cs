using UnityEngine;
using System.Collections;

public class AwakeInit : Singleton<AwakeInit>
{
    //게임 시작시 오프닝 호출.
    public void Awake()
    {
        Handheld.PlayFullScreenMovie("opening 1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        Screen.SetResolution(2560, 1334, true);
    }
}
