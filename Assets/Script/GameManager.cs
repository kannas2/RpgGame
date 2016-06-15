using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
	public void Awake()
    {
        Handheld.PlayFullScreenMovie("opening 1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        Screen.SetResolution(1280, 800, true);
    }
}
