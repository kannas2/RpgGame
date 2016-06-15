using UnityEngine;
using System.Collections;

public class AttackSkill : BaseSkill
{
    public static AttackSkill instance;

	void Start ()
    {
        instance = this;
        base.attackSpeed = 1.0f;
        base.coolTime = 1.0f;
        base.curCoolTime = .0f;

        base.minDamage = 20;
        base.maxDamage = 30;

        base.player = PlayerCtrl.instance;
        base.swordeffectpath = "Prefab/Skill/StormSlash";
        base.boosterAttack = false;
    }

    void FixedUpdate()
    {
        base.CoolTime(time);
        button.fillAmount = curCoolTime / coolTime;
    }

    public override void Attack()
    {
        if (skillState != false)
        {
            if (boosterAttack)
            {
                curCoolTime = .0f;
                attackSpeed = 2.0f;
                swordeffectpath = "Prefab/Skill/ArcaneSlash";
            }
            else
            {
                curCoolTime = 1.0f;
                attackSpeed = 1.0f;
                swordeffectpath = "Prefab/Skill/StormSlash";
            }

            player.curMP += 2;
            player.curAttackPower = base.RandomDamage(minDamage, maxDamage);
            player.anim.SetTrigger("Attack");

            StartCoroutine(SwordEffect(.0f, 1));
            StartCoroutine(player.AttackCount(1));
        }
    }
}