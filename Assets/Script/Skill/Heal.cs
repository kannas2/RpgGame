using UnityEngine;
using System.Collections;

public class Heal : BaseSkill {

    public static Heal instance;

	void Start ()
    {
        instance        = this;
        base.coolTime    = 45.0f;
        base.curCoolTime = .0f;
        base.healValue   = 20.0f;
        base.skillState  = true;
        base.effectpath  = "Prefab/Skill/Heal";
        base.useMp = 20.0f;

        base.GetComponent();

    }
	
	void Update ()
    {
        base.CoolTime(time);
        button.fillAmount = curCoolTime / coolTime;
	}

    public override void Buff()
    {
        if (skillState != false && player.curMP >= useMp)
        {
            curCoolTime = 45.0f;

            player.curMP -= useMp;
            base.PaticleEffect(); //파티클 추후 조정예정.
            PlayerCtrl.instance.anim.SetTrigger("CastSpell"); //모션
            PlayerCtrl.instance.curHP += healValue;
            PlayerCtrl.instance.curMP += healValue;
        }
    }
}
