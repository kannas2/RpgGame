using UnityEngine;
using System.Collections;
using System;

public class State_Die : FSM_State<Monster>
{
    public static readonly State_Die instance = new State_Die();
    public static State_Die Instance
    {
        get
        {
            return instance;
        }
    }

    public float count;
    public float time;

    public override void EnterState(Monster monster)
    {
        count = 2.0f;
        time = .0f;

        monster.anim.SetTrigger("die");
        //죽음
        monster.IsDead = true;
        //아이템생성
        monster.Create_item();
    }

    public override void UpdateState(Monster monster)
    {
        time += Time.deltaTime;
        //서서히 사라지게끔 설정.
        if (time >= 2.0f)
        {
            monster.gameObject.SetActive(false);
            time = .0f;
        }
            
    }

    public override void ExitState(Monster _monster)
    {
    }
}
