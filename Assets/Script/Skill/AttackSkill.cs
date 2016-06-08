using UnityEngine;
using System.Collections;

public class AttackSkill : BaseSkill
{
    public static AttackSkill instance;

	void Start ()
    {
        instance = this;
        base.attackSpeed = 1.0f;

        base.minDamage = 20;
        base.maxDamage = 30;

        base.player = PlayerCtrl.instance;
	}

    public override void Attack()
    {
        player.curMP += 2;
        player.curAttackPower = base.RandomDamage(minDamage, maxDamage);
        player.anim.SetTrigger("Attack");
        StartCoroutine(player.AttackCount(1));
    }
}