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

    public override void EnterState(Monster monster)
    {
        if (monster.myTarget == null)
        {
            monster.ChangeState(State_Move.instance); //StateMove에 제자리로 돌아가는 기능이 있음.
            return;
        }

        monster.projectAttackTimer = .0f;
    }

    public override void UpdateState(Monster monster)
    {
        if(monster.curHP <= 0)
        {
            monster.ChangeState(State_Die.instance);
        }
        
        monster.projectAttackTimer += 1.0f * Time.deltaTime;

         if(monster.projectAttackTimer >= monster.projecttileAttackSpeed
            && !monster.player.isDead)
        {
            // 캐릭터의 방향으로 전환 다른 때는 상관없는데 이건 특정장소로 이동하고나서 공격하는거라 Look 해줘야함.
            monster.PlayerLook();

                monster.chaseTime = .0f;
                //원거리 공격 구체 생성. 구체는 생성될때 플레이어의 초기 위치를 딱 한번 대입받고
                // 해당위치로 이동 한다. 지면에 혹은 일정거리 이상 날라갈 시에 터지는 파티클 생성후 삭제
        }
        else //생성된 구체가 5개 이상이라면
        {
            //다른 행동으로 전환 ex ) monster.ChangeState(State_Move.instance);
            //혹은 구체 발사하는 지점이 공중이라면 FlyMove FlyMove에서 일정거리 도달하면 StateMOve로 전환.
        }
    }

    public override void ExitState(Monster monster)
    {

    }
}
