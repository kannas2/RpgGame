using UnityEngine;
using System.Collections;

public class MonsterAngel : Monster {

	void Start ()
    {
        base.curHP = 50.0f;
        base.maxHP = 50.0f;

        base.minDamage = 10;
        base.maxDamage = 15;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 2.0f;  //어택 딜레이타임이 더 맞는 말인듯.

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 5.0f;

        base.attackState = false;
        base.preMonsterPos = transform.position;

        base.attackDis = 0.9f;
        base.checkDis = 2.5f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.attack = false;
        base.monsterName.text = "아가사";
        base.itemPath = "Prefab/Item/medalA";
        base.itemName = "medalA";

        base.attackTimer = .0f;
        base.dieTime = .0f;
    }
	
	void FixedUpdate()
    {
        state.Update();
        base.MonsterUpdateHP();
	}

    //물리적 충돌X
    void OnTriggerEnter(Collider coll)
    {
        base.TriggerEnter(coll);
    }

    void OnTriggerExit(Collider coll)
    {
        attack = true;
    }
}
