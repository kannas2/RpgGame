using UnityEngine;
using System.Collections;

public class MonsterBunny : Monster {

	void Start ()
    {
        base.curHP = 150;
        base.maxHP = 150;

        base.startDamage = 20.0f;
        base.maxDamage = 25;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 5.0f;

        base.attackState = false;
        base.preMonsterPos = transform.position; //시작위치.
        base.curMonsterPos = transform;

        base.attackDis = 2.5f;
        base.IsDead = false;

        base.itemPath = "Prefab/DropItem";

        if (state == null)
        {
            base.state = new State_Machine<Monster>();
        }

        base.GetComponent();
        base.anim = transform.GetComponent<Animator>();

        base.ResetState();
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        curMonsterPos = transform; //몬스터의 현재 위치.
        state.Update();
        //base.MonsterUpdateHP();
	}
}
