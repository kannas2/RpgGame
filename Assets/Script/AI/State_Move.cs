using UnityEngine;
using System.Collections;
using System;

public class State_Move : FSM_State<Monster>
{
    public static readonly State_Move instance = new State_Move();
    public static State_Move Instance
    {
        get
        {
            return instance;
        }
    }

    static State_Move() { }
    private State_Move() { }

    public override void EnterState(Monster monster)
    {

    }

    public override void UpdateState(Monster monster)
    {
        if (monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        if (monster.myTarget != null)
        {
            monster.anim.SetBool("Walk", true);
            monster.attack = false;

            if (monster.CheckRange() >= monster.attackDis)
            {
                monster.chaseTime += Time.deltaTime;
                if (monster.chaseTime >= monster.chaseCancleTime)
                {
                    monster.myTarget = null;
                    monster.chaseTime = .0f;
                    return;
                }
                monster.MonsterMove(monster.myTarget.position);
            }
            else
            {
                monster.anim.SetBool("Walk", false);
                monster.attack = true;
                monster.ChangeState(State_Attack.Instance);
            }
        }
        else
        {
            //몇초이내에 원래의 자리로 못돌아갈경우 다시 몬스터를 세팅.
            monster.MonsterMoveReset();
        }
    }

    public override void ExitState(Monster monster)
    {
        monster.anim.SetBool("Walk", false);
        //Debug.Log("워크 정정");
    }
}
