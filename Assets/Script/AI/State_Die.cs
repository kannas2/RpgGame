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

    public override void EnterState(Monster monster)
    {
        monster.dieTime = .0f;

        for (int i = 0; i < monster.checkbox.Length; i++)
        {
            monster.checkbox[i].enabled = false;
        }
        monster.anim.SetTrigger("Die");
        monster.IsDead = true;
        monster.Create_item();
    }

    public override void UpdateState(Monster monster)
    {
        monster.dieTime += Time.deltaTime;
        
        if (monster.dieTime >= 5.0f)
        {
            monster.dieTime = .0f;
            monster.MonsterDie();
        }
            
    }

    public override void ExitState(Monster _monster)
    {
    }
}
