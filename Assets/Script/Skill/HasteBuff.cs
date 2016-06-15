using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HasteBuff : BaseSkill {

    public static HasteBuff instance;

	void Start ()
    {
        instance          = this;
        base.buffTime      = 30.0f;
        base.coolTime      = 60.0f;
        base.curCoolTime   = 0.0f;
        base.speedValue    = 5.0f;
        base.skillState    = true;
        base.buffeffectpath = "Prefab/Skill/Hest";
        base.useMp = 15.0f;

        base.GetComponent();
    }
	
	void FixedUpdate()
    {
        base.CoolTime(time);
        button.fillAmount = curCoolTime / coolTime;
	}

    public override void Buff()
    {
        if (skillState != false && player.curMP >= useMp)
        {
            curCoolTime = 60.0f;
            player.curMP -= base.useMp;
            base.PaticleEffect(topBuffeffectPos, 2.5f);
            player.anim.Play("CastSpell");
            StartCoroutine(base.Buffwait("Hast", buffTime));
        }
    }
}