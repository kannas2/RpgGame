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
        PlayerCtrl.instance.anim.SetBool("isAttack", true);
        PlayerCtrl.instance.attackChk = true;
        //애니메이션 동작 속도 설정하면 될듯 여기에.
        //쿨타임은 조이스틱이나 여기서 해결하던가 해야할거 같고.
    }
}
