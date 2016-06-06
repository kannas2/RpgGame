using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Brandish : BaseSkill {

    public static Brandish instance;

	void Start ()
    {
        instance = this;

        base.coolTime    = 2.0f;
        base.curCoolTime =  .0f;
        base.attackSpeed = 2.0f;
        base.damageValue = 10.0f;
        base.skillState  = true;

        base.GetComponent();
	}

	void FixedUpdate()
    {
        base.CoolTime(time);
        button.fillAmount = curCoolTime / coolTime;
	}

    //데미지를 어떻게 전달 할 것인가.
    public override void Attack()
    {
        if (skillState != false)
        {
            curCoolTime = 2.0f;
            PlayerCtrl.instance.curAttackPower = damageValue; //데미지.
            PlayerCtrl.instance.anim.SetTrigger("Brandish");
        }
    }
}
