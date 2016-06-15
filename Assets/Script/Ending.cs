using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    public float time;

	void Awake()
    {
        Handheld.PlayFullScreenMovie("ending 1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        time = .0f;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ScreenEvent();
        }

        time += 1.0f * Time.deltaTime;
        if(time > 54)
        {
            ScreenEvent();
        }
    }

    public void ScreenEvent()
    {
        Application.LoadLevel(0);
    }
}
