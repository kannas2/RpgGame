using UnityEngine;
using System.Collections;
using System;

public class ProjectileMove : FSM_State<Monster>
{
    public static readonly ProjectileMove instance = new ProjectileMove();
    public static ProjectileMove Instance
    {
        get
        {
            return instance;
        }
    }

    private float distance;
    private int   rand;

    public override void EnterState(Monster monster)
    {
        rand = UnityEngine.Random.Range(0, 2);
        //몬스터 공중 idle로 변경 드래곤일 경우.
        monster.anim.SetBool("Fly Idle", true);
        Debug.Log("Rand :" + rand);
    }

    public override void UpdateState(Monster monster)
    {
        if (monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        if (monster.myTarget != null)
        {
            monster.anim.SetBool("Fly Forward", true);
            distance = Vector3.Distance(monster.pivotPos[rand], monster.transform.position);

            if (distance >= 0.1f)
            {
                monster.MonsterMove(monster.pivotPos[rand]);
            }
            else
            {
                monster.anim.SetBool("Fly Forward", false);
                monster.ChangeState(ProjectileAttack.instance);
                Debug.Log("도착 함");
            }
        }
        else
        {
            monster.MonsterMoveReset();
        }
    }
    public override void ExitState(Monster monster)
    {
        monster.anim.SetBool("Fly Forward", false);
    }
}
