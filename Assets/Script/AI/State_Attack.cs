using UnityEngine;
using System.Collections;

public class State_Attack : FSM_State<Monster>
{
    static readonly State_Attack instance = new State_Attack();
    public static State_Attack Instance
    {
        get
        {
            return instance;
        }
    }

    private float attack_timer = .0f;
    
    static State_Attack() { }
    private State_Attack() { }

    public override void EnterState(Monster _monster)
    {
        Debug.Log("어택 시작");
        if (_monster.my_target == null)
        {
            return;
        }
        attack_timer = _monster.attack_speed;
    }

    public override void UpdateState(Monster _monster)
    {
        if(_monster.current_HP <= 0)
        {
            _monster.ChangeState(State_Die.Instance);
        }
        attack_timer += Time.deltaTime;
        if(!_monster.my_target.GetComponent<PlayerCtrl>().isDead && _monster.Check_Range() && _monster.Check_Angle())
        {
            if(attack_timer >= _monster.attack_speed)
            {
                //데미지
                //Debug.Log("플레이어 데미지 입힘");
                //_monster.player.Char_Update_HP(0.2f);

                //몬스터 애니메이션
                _monster.GetComponent<Animation>().CrossFade("attack_2", 0.2f);
                attack_timer = .0f;
                _monster.chase_time = .0f;
            }
        }
        else
        {
            //이동
            _monster.ChangeState(State_Move.Instance);
        }
    }

    public override void ExitState(Monster _monster)
    {
        Debug.Log("State_Attack_out");
    }
}