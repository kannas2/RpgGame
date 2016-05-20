using UnityEngine;
using System.Collections.Generic;
using System;

public class State_Machine <t>
{
    private t Owner;
    private FSM_State<t> Current_State;
    private FSM_State<t> Previous_State;

    public void Awake()
    {
        Current_State = null;
        Previous_State = null;
    }

    public void ChangeState(FSM_State<t> _NewState)
    {
        if(_NewState == Current_State)
        {
            return;
        }
        //이전 상태를 저장.
        Previous_State = Current_State;

        if(Current_State != null)
        {
            Current_State.ExitState(Owner);
        }
        Current_State = _NewState;

        if(Current_State != null)
        {
            Current_State.EnterState(Owner);
        }
    }

    public void Initial_Setting(t _Owner, FSM_State<t> _InitialState)
    {
        Owner = _Owner;
        ChangeState(_InitialState);
    }

    public void Update()
    {
        if(Current_State != null)
        {
            Current_State.UpdateState(Owner);
        }
    }
    public void StateRevert()
    {
        if(Previous_State != null)
        {
            ChangeState(Previous_State);
        }
    }
}
