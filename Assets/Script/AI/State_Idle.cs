using UnityEngine;
using System.Collections;
using System;

public class State_Idle : FSM_State<Monster>
{
    public static readonly State_Idle instance = new State_Idle();
    public static State_Idle Instance
    {
        get
        {
            return instance;
        }
    }

    public override void EnterState(Monster monster)
    {
    }

    public override void UpdateState(Monster monster)
    {
        monster.anim.SetTrigger("Idle");

        if (monster.CheckRange() <= monster.checkDis) //거리.
        {
            monster.ChangeState(State_Move.Instance);
        }
    }

    public override void ExitState(Monster monster)
    {
    }
}
/*
string 값으로 vector3 값을 받아서 string을 벡터에 넣어주는 형식으로
3개씩 입력받고 다음 idx로 넘어가는 형식으로 구현을 하면 될것 같은데..
몬스터의 종류 / pos / 그외  struct로 받아서 넣는 방식으로.
    */