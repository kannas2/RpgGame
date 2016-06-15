using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuffState : MonoBehaviour {

    public string buffname;
    public Image  buffImage;
    public float  buffTime;

    public virtual void BuffSetting(float sec)
    {
        buffTime -= 1.0f * Time.deltaTime;
        buffImage.fillAmount = buffTime / sec;
    }
}
