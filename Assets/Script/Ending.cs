using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

    public float time;

    //엔딩 영상 호출.
	void Awake()
    {
        Handheld.PlayFullScreenMovie("ending 1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        time = .0f;
    }

    //메인화면으로 가는 이벤트 처리.
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ScreenEvent();
        }

        time += 1.0f * Time.deltaTime;

        if (time > 54)
        {   
            ScreenEvent();
        }
    }

    public void ScreenEvent()
    {
        Application.LoadLevel(0);
    }
}
