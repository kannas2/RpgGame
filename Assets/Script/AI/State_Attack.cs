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
        else if(monster.type == Monster.MonsterType.Strong || 
               monster.type == Monster.MonsterType.Boss && monster.curHP <= 200.0f )
        {
            //이동
            //다른 곳으로 이동 원거리 공격 할 만한 곳으로. 그 이동하는 상태 스크립트에서 1~3 정도 rand돌려서 방향 잡아서 그곳으로 이동시키고 그다음 ProejctAttack으로 change.
            //나중에 특정 보스만이 취할수 있는 행동을 할때 일시적인 확률로 본체 힐을 할 수 있게 본체힐을 한 후에 그전 행동으로 돌아가는 StateRevert() 를 호출할것.
        }
        else
        {
            monster.ChangeState(State_Move.instance);
        }
    }

    public override void ExitState(Monster monster)
    {
    }
}