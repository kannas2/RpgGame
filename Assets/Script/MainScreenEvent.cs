using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainScreenEvent : MonoBehaviour {

	void Awake ()
    {
        Screen.SetResolution(1280, 800, true);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ScreenEvent();
        }
    }

    public void ScreenEvent()
    {
        Application.LoadLevel(1);
    }
}
