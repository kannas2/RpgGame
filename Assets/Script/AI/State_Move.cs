using UnityEngine;
using System.Collections;
using System;

public class State_Move : FSM_State<Monster>
{
    static readonly State_Move instance = new State_Move();
    public static State_Move Instance
    {
        get
        {
            return instance;
        }
    }

    private float reset_time = 3f;
    private float current_time;

    private Vector3 _dir;
    private Vector3 _nordir;
    private float distance;

    static State_Move() { }
    private State_Move() { }

    public override void EnterState(Monster _monster)
    {
        current_time = reset_time;
    }

    public override void UpdateState(Monster _monster)
    {
        if (_monster.current_HP <= 0)
        {
            _monster.ChangeState(State_Die.Instance);
        }

        if (_monster.my_target != null)
        {
            if (!_monster.Check_Range())
            {
                _monster.chase_time += Time.deltaTime;
                if (_monster.chase_time >= _monster.chase_cancle_time)
                {
                    _monster.my_target = null;
                    _monster.chase_time = .0f;
                    return;
                }

                //회전각 (플레이어가 있는 방향을 정규화시켜서 설정)
                _dir = _monster.my_target.position - _monster.transform.position;
                _nordir = _dir.normalized;

                Quaternion angle = Quaternion.LookRotation(_nordir);
                _monster.transform.rotation = angle;

                //방향은 위에서 설정했으니 앞으로 가기만 하면됨.
                Vector3 pos = _monster.transform.position;
                pos += _monster.transform.forward * Time.smoothDeltaTime * _monster.move_speed;
                _monster.transform.position = pos;

                _monster.ani.CrossFade("walk");
            }
            else
            {
                _monster.ChangeState(State_Attack.Instance);
            }
        }
        else
        {
            //몇초이내에 원래의 자리로 못돌아갈경우 다시 몬스터를 세팅.
            Monster_Reset(_monster);
        }
    }

    public void Monster_Reset(Monster _monster)
    {
        //거리 계산
        distance = Vector3.Distance(_monster.transform.position, _monster.set_monster_pos);
        
        //방향
        _dir = _monster.set_monster_pos - _monster.transform.position;
        _nordir = _dir.normalized;

        _monster.transform.rotation = Quaternion.LookRotation(_nordir);

        if (distance >= 0.1f)
        {
            Vector3 pos = _monster.transform.position;
            pos += _monster.transform.forward * Time.smoothDeltaTime * 4.0f;
            _monster.transform.position = pos;

            _monster.ani.CrossFade("walk");
        }
        else
        {
            _monster.ResetState();
        }
        Debug.Log("제자리로 이동.");
    }

    public override void ExitState(Monster _monster)
    {
        Debug.Log("State_Move 종료");
    }
}
