using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseSkill : MonoBehaviour {

    //캐릭터 스킬에 필요한 것들...
    //쿨타임, 버프 시간, 버프 맥스시간,버프(힐, 이동속도), 공격속도, 데미지,  
    public float buffTime;
    public float buffLimitTime;
    public float buffCoolTime;
    public float curbuffCoolTime;
    public float healValue;
    public float speedValue;
    public bool  buffState;
    public Image button;

    public float attackSpeed;
    public float damageValue;

    public virtual void Heal() { }
    public virtual void Attack() { }
    public virtual void Buff() { }

    public virtual IEnumerator Buffwait(string buff,float ftime)
    {
        switch (buff)
        {
            case "Hast":
                {
                    SmallHast.instance.buffTime = 30.0f;
                    PlayerCtrl.instance.curMoveSpeed = 6.0f;
                    yield return new WaitForSeconds(ftime);
                    PlayerCtrl.instance.curMoveSpeed = PlayerCtrl.instance.baseMoveSpeed;
                }
                break;
        }
    }
    
    public void CoolTime()
    {
        //쿨타임이 60초인데 0보다 작거나 같아지면 스킬을 사용할수 있게 설정.
        if (curbuffCoolTime <= .0f)
        {
            buffState = true;
            curbuffCoolTime = .0f;
        }
        else if (curbuffCoolTime >= .0f)// 0보다 클경우 값을 1초씩 감산. 스킬을 사용못함.
        {
            curbuffCoolTime -= 1.0f * Time.deltaTime;
            buffState = false;
        }
    }
}
