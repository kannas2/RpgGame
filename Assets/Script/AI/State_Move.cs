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

    private float resetTime;
    private float currentTime;

    private Vector3 dir;
    private Vector3 nordir;
    private float distance;

    static State_Move() { }
    private State_Move() { }

    public override void EnterState(Monster monster)
    {
        resetTime = 3.0f;
        currentTime = resetTime;
    }

    public override void UpdateState(Monster monster)
    {
        if (monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.Instance);
        }

        if (monster.myTarget != null)
        {
            if (!monster.Check_Range())
            {
                monster.chaseTime += Time.deltaTime;
                if (monster.chaseTime >= monster.chaseCancleTime)
                {
                    monster.myTarget = null;
                    monster.chaseTime = .0f;
                    return;
                }

                //회전각 (플레이어가 있는 방향을 정규화시켜서 설정)
                dir = monster.myTarget.position - monster.curMonsterPos.position;
                nordir = dir.normalized;

                Quaternion angle = Quaternion.LookRotation(nordir);
                monster.curMonsterPos.rotation = angle;

                //방향은 위에서 설정했으니 앞으로 가기만 하면됨.
                Vector3 pos = monster.curMonsterPos.position;
                pos += monster.curMonsterPos.forward * Time.smoothDeltaTime * monster.curMoveSpeed;
                monster.curMonsterPos.position = pos;

                monster.anim.SetTrigger("walk"); //애니메이션 수정 해야할것.,
            }
            else
            {
                monster.ChangeState(State_Attack.Instance);
            }
        }
        else
        {
            //몇초이내에 원래의 자리로 못돌아갈경우 다시 몬스터를 세팅.
            Monster_Reset(monster);
        }
    }

    public void Monster_Reset(Monster monster)
    {
        //거리 계산
        distance = Vector3.Distance(monster.curMonsterPos.position, monster.preMonsterPos);
        
        //방향
        dir = monster.preMonsterPos - monster.curMonsterPos.position;
        nordir = dir.normalized;

        monster.curMonsterPos.rotation = Quaternion.LookRotation(nordir);

        if (distance >= 0.1f)
        {
            Vector3 pos = monster.curMonsterPos.position;
            pos += monster.curMonsterPos.forward * Time.smoothDeltaTime * 4.0f;
            monster.curMonsterPos.position = pos;

            monster.anim.SetTrigger("walk");
        }
        else
        {
            monster.ResetState();
        }
    }

    public override void ExitState(Monster monster)
    {
        Debug.Log("State_Move 종료");
    }
}
