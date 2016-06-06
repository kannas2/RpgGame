using UnityEngine;
using System.Collections.Generic;
using System;

public class State_Machine <t>
{
    private t Owner;
    private FSM_State<t> CurrentState;
    private FSM_State<t> PreviousState;

    public void Awake()
    {
        CurrentState = null;
        PreviousState = null;
    }

    public void ChangeState(FSM_State<t> _NewState)
    {
        if(_NewState == CurrentState)
        {
            return;
        }
        PreviousState = CurrentState;

        if(CurrentState != null)
        {
            CurrentState.ExitState(Owner);
        }
        CurrentState = _NewState;

        if(CurrentState != null)
        {
            CurrentState.EnterState(Owner);
        }
    }

    public void Initial_Setting(t _Owner, FSM_State<t> _InitialState)
    {
        Owner = _Owner;
        ChangeState(_InitialState);
    }

    public void Update()
    {
        if(CurrentState != null)
        {
            CurrentState.UpdateState(Owner);
        }
    }
    public void StateRevert()
    {
        if(PreviousState != null)
        {
            ChangeState(PreviousState);
        }
    }
}
