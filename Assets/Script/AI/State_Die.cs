using UnityEngine;
using System.Collections;
using System;

public class State_Die : FSM_State<Monster>
{
    static readonly State_Die instance = new State_Die();
    public static State_Die Instance
    {
        get
        {
            return instance;
        }
    }
    float count = 2.0f;
    float time = .0f;

    public override void EnterState(Monster _monster)
    {
        //몬스터 죽은 애니메이션 설정
        _monster.GetComponent<Animation>().CrossFade("death");
        //죽음
        _monster.IsDead = true;
        //아이템생성
        _monster.Create_item();
    }

    public override void UpdateState(Monster _monster)
    {
        time += Time.deltaTime;
        //서서히 사라지게끔 설정.
        if (time >= 2.0f)
        {
            _monster.gameObject.SetActive(false);
            time = .0f;
        }
            
    }

    public override void ExitState(Monster _monster)
    {
    }
}
