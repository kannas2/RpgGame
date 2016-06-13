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

        base.minDamage = 40;
        base.maxDamage = 60;

        base.skillState  = true;
        base.useMp = 10.0f;
        base.swordeffectpath = "Prefab/Skill/FireSlash";
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
        if (skillState != false && player.curMP >= base.useMp)
        {
            curCoolTime = 4.0f;
            player.curMP -= useMp;
            player.curAttackPower = base.RandomDamage(minDamage,maxDamage); //데미지.
            player.anim.SetTrigger("Brandish");
            StartCoroutine(SwordEffect(0.3f, 2));
            StartCoroutine(PlayerCtrl.instance.AttackCount(2));
        }
    }
}
