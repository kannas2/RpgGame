using UnityEngine;
using System.Collections;
using System;

public class State_Idle : FSM_State<Monster>
{
    static readonly State_Idle instance = new State_Idle();
    public static State_Idle Instance
    {
        get
        {
            return instance;
        }
    }

    public override void EnterState(Monster _monster)
    {
    }

    public override void UpdateState(Monster _monster)
    {
        if(_monster.Check_Range())
        {
            _monster.ChangeState(State_Move.Instance);
        }
        _monster.ani.CrossFade("idle");
    }

    public override void ExitState(Monster _monster)
    {
    }
}
/*
string 값으로 vector3 값을 받아서 string을 벡터에 넣어주는 형식으로
3개씩 입력받고 다음 idx로 넘어가는 형식으로 구현을 하면 될것 같은데..
몬스터의 종류 / pos / 그외  struct로 받아서 넣는 방식으로.
    */