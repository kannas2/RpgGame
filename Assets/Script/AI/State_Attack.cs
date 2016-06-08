using UnityEngine;
using System.Collections;

public class State_Attack : FSM_State<Monster>
{
    public static readonly State_Attack instance = new State_Attack();
    public static State_Attack Instance
    {
        get
        {
            return instance;
        }
    }

    //private float attackTimer;
    
    static State_Attack() { }
    private State_Attack() { }

    public override void EnterState(Monster monster)
    {
        if (monster.myTarget == null)
        {
            return;
        }
        monster.attackTimer = .0f;
        monster.attack = true;
    }

    public override void UpdateState(Monster monster)
    {
        if(monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        monster.attackTimer += 1.0f * Time.deltaTime;
        if(!monster.player.isDead && monster.CheckRange() <= monster.attackDis && monster.Check_Angle()) //나중에 외적Angle 아니다 싶으면 빼는걸로.
        {
            if(monster.attackTimer >= monster.curAttackSpeed)
            {
                //데미지
                monster.anim.SetTrigger("attack");

                monster.attackTimer = .0f;
                monster.chaseTime = .0f;
            }
        }
        else
        {
            //이동
            monster.ChangeState(State_Move.Instance);
        }
    }

    public override void ExitState(Monster monster)
    {
    }
}