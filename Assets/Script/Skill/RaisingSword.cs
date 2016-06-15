using UnityEngine;
using System.Collections;

public class RaisingSword : BaseSkill {

    public static RaisingSword instance;

    void Start()
    {
        instance = this;
        base.buffTime = 30.0f;
        base.coolTime = 60.0f;
        base.curCoolTime = .0f;
        base.skillState = true;
        base.buffeffectpath = "Prefab/Skill/BoostAttack";
        base.swordeffectpath = "Prefab/Skill/ArcaneSlash";
        base.useMp = 30.0f;

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
            player.curMP -= useMp;
            base.PaticleEffect(topBuffeffectPos, 2.5f);
            player.anim.SetTrigger("CastSpell");
            StartCoroutine(base.Buffwait("Booster", buffTime));
        }
    }

}
