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

    private float attackTimer;
    
    static State_Attack() { }
    private State_Attack() { }

    public override void EnterState(Monster monster)
    {
        if (monster.myTarget == null)
        {
            return;
        }
        attackTimer = monster.curAttackSpeed;
    }

    public override void UpdateState(Monster monster)
    {
        if(monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        attackTimer += Time.deltaTime;
        if(!monster.player.isDead && monster.CheckRange() <= monster.attackDis && monster.Check_Angle()) //나중에 외적Angle 아니다 싶으면 빼는걸로.
        {
            if(attackTimer >= monster.curAttackSpeed)
            {
                //데미지
                //Debug.Log("플레이어 데미지 입힘");
                //_monster.player.Char_Update_HP(0.2f);

                //몬스터 애니메이션
                monster.anim.SetTrigger("attack");
                attackTimer = .0f;
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