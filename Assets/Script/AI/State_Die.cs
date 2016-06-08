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

        monster.checkbox.enabled = false;
        monster.anim.SetTrigger("die");
        monster.IsDead = true;
        monster.Create_item();
    }

    public override void UpdateState(Monster monster)
    {
        time += Time.deltaTime;
        
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
