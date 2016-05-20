using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainScreenEvent : MonoBehaviour {

    public SpriteRenderer MainStorySprite;
    public int StoryIndex;
    public Transform camera;

	void Start ()
    {
        camera.transform.position = new Vector3(.0f, .0f, -10.0f);
        MainStorySprite = transform.FindChild("Story").GetComponent<SpriteRenderer>();
        StoryIndex = 0;
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
        if (StoryIndex < 5)
        {
            camera.transform.position = new Vector3(12.8f, .0f, -10.0f);
            MainStorySprite.sprite = Resources.Load("BackGround/Story" + StoryIndex, typeof(Sprite)) as Sprite;
        }
        else
        {
            Debug.Log("게임 시작.");
        }
        StoryIndex++;
    }
}
