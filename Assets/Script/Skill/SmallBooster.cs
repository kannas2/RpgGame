using UnityEngine;
using UnityEngine.UI;

public class SmallBooster : BuffState {

    public static SmallBooster instance;

    void Start ()
    {
        instance = this;
        base.buffname = "GameUI/interface/sSword";
        base.buffImage = transform.GetComponent<Image>();
        base.buffTime = .0f;
        buffImage.sprite = Resources.Load(buffname, typeof(Sprite)) as Sprite;

    }
	
	void Update ()
    {
	    if(buffTime <= 0)
        {
            buffTime = .0f;
        }

        base.BuffSetting(30.0f);
	}
}
