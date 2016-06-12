using UnityEngine;
using System.Collections;

public class MonsterBunny : Monster {

	void Start ()
    {
        base.curHP = 150.0f;
        base.maxHP = 150.0f;

        base.minDamage = 20;
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

        base.attackDis = 0.9f; //플레이어 공격 거리  
        base.checkDis = 2.5f;  //플레이어 인식 거리. 
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        //Getcomponent이후.
        base.attack= false;
        base.monsterName.text = "래 피";
        base.itemPath = "Prefab/Item/medalC";
        base.itemName = "medalC";

        base.type = MonsterType.Weak;
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