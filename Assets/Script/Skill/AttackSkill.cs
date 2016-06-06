using UnityEngine;
using System.Collections;

public class AttackSkill : BaseSkill
{
    public static AttackSkill instance;

	void Start ()
    {
        instance = this;
        base.attackSpeed = 1.0f;
        base.damageValue = 1.0f;
	}

    public override void Attack()
    {
        PlayerCtrl.instance.anim.SetTrigger("Attack");
        PlayerCtrl.instance.attackChk = true;
    }
}