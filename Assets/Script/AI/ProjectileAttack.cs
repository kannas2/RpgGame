using UnityEngine;
using System.Collections;

public class ProjectileAttack : FSM_State<Monster>
{
    /*
    구체를 생성하여 플레이어에게 날리는 공격을 수행하는 스크립트.
    정예~ 보스 몬스터만 사용하는 스크립트로 몬스터의 Type를 검사해야 들어올수 있는 스크립트
    몬스터가 원거리 공격을 하는 Timer도 정에~보스 몬스터에만 존재함.
    */

    public static readonly ProjectileAttack instance = new ProjectileAttack();
    public static ProjectileAttack Instance
    {
        get
        {
            return instance;
        }
    }
    private int attackCNT;

    public override void EnterState(Monster monster)
    {
        if (monster.myTarget == null)
        {
            monster.ChangeState(State_Move.instance); //StateMove에 제자리로 돌아가는 기능이 있음.
            return;
        }

        monster.projectAttackTimer = .0f;
        monster.projectileCNT = 0;
        attackCNT = 5;

        //몬스터가 공격하는 횟수도 랜덤으로 할까 ex) monster.randattack = Random(min, max); 나중에 물어보고..결정 하는걸로..
    }
    
    public override void UpdateState(Monster monster)
    {
        if(monster.curHP <= 0)
        {
            monster.anim.SetBool("Fly Idle", false);
            monster.ChangeState(State_Die.instance);
        }
        
        monster.projectAttackTimer += 1.0f * Time.deltaTime;

        if(!monster.player.isDead && monster.projectileCNT < attackCNT)
        {
            monster.PlayerLook();
            if (monster.projectAttackTimer >= monster.projecttileAttackSpeed && monster.Check_Angle())
            {
                // 캐릭터의 방향으로 전환 다른 때는 상관없는데 이건 특정장소로 이동하고나서 공격하는거라 Look 해줘야함.
                monster.anim.SetTrigger("Fly Projectile Attack");

                //원거리 공격 구체 생성. 
                monster.CreateBullet(monster.projectilePath, 1.0f);
                monster.projectAttackTimer = .0f;
                monster.chaseTime = .0f;
                monster.projectileCNT++; //공격 카운트
            }
        }
        else if(monster.projectAttackTimer >=1.5f && monster.projectileCNT >= 5)
        {
            monster.anim.SetBool("Fly Idle", false);
            monster.ChangeState(State_Move.instance);
        }
    }

    public override void ExitState(Monster monster)
    {
        monster.anim.SetBool("Fly Idle", false);
    }
}
