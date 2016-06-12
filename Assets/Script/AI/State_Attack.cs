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

        //몬스터 Walk가 혹시 플레이중이라면 끄는off.
        monster.stateInfo = monster.anim.GetCurrentAnimatorStateInfo(0);
        if(monster.stateInfo.IsName("Walk"))
        {
            monster.anim.SetBool("Walk", false);
        }
    }

    public override void UpdateState(Monster monster)
    {
        if(monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        monster.attackTimer += 1.0f * Time.deltaTime;
        monster.projecttileCoolTime += 1.0f * Time.deltaTime;
        monster.healCoolTime += 1.0f * Time.deltaTime;
        monster.PlayerLook();

        if (!monster.player.isDead && monster.CheckRange() <= monster.attackDis && monster.Check_Angle())
        {
            if (monster.attackTimer >= monster.curAttackSpeed)
            {
                //데미지
                monster.anim.SetTrigger("Attack");

                monster.attackTimer = .0f;
                monster.chaseTime = .0f;
                monster.attack = true;
            }

            if(monster.type == Monster.MonsterType.Boss && monster.healCoolTime >= 10.0f)
            {
                monster.healCoolTime = .0f;
                monster.MosnterHeal();
            }

            if (monster.type == Monster.MonsterType.Strong || monster.type == Monster.MonsterType.Boss)
            {
                if (monster.curHP <= 200.0f && monster.projecttileCoolTime >= 20)
                {
                    monster.projecttileCoolTime = .0f;
                    monster.ChangeState(ProjectileMove.instance);
                    //나중에 특정 보스만이 취할수 있는 행동을 할때 일시적인 확률로 본체 힐을 할 수 있게 본체힐을 한 후에 그전 행동으로 돌아가는 StateRevert() 를 호출할것.
                }
            }
        }
        else
        {
            monster.ChangeState(State_Move.instance);
        }
    }

    public override void ExitState(Monster monster)
    {
        //monster.attack = false;
    }
}