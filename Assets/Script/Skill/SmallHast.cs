using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SmallHast : BuffState {

    public static SmallHast instance;

	void Start ()
    {
        instance = this;
        base.buffname = "GameUI/use";
        base.buffImage = transform.GetComponent<Image>();
        base.buffTime = 0;
        buffImage.sprite = Resources.Load(buffname, typeof(Sprite)) as Sprite;
    }

    void Update ()
    {
        if(buffTime <= 0)
        {
            buffTime = .0f;
        }

        base.BuffSetting();
    }
}
