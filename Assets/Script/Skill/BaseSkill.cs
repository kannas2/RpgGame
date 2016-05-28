using UnityEngine;

public class BaseSkill : MonoBehaviour {

    //캐릭터 스킬에 필요한 것들...
    //쿨타임, 버프 시간, 버프 맥스시간,버프(힐, 이동속도), 공격속도, 데미지,  
    public float buffTime;
    public float buffLimitTime;
    public float buffCoolTime;
    public float healValue;
    public float speedValue;

    public float attackSpeed;
    public float damageValue;

    public virtual void Heal() { }
    public virtual void Attack() { }
    public virtual void Buff() { }
    public virtual void CoolTime() { }
}
