﻿using UnityEngine;
using System.Collections;

public class MonsterDragon : Monster {

	void Start ()
    {
        base.curHP = 300.0f;
        base.maxHP = 300.0f;

        base.minDamage = 30;
        base.maxDamage = 40;

        base.minSkllDamage = 50;
        base.maxSkillDamage = 60;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 8.0f;

        base.attackState = true;
        base.preMonsterPos = transform.position;
        base.curMonsterPos = transform;

        base.attackDis = 1.0f; //나중에 보고 조절할것.
        base.checkDis = 3.0f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.attack = false;
        base.monsterName.text = "드래곤";
        base.itemPath = "Prefab/Item/dragonHorn";
        base.itemName = "dragonHorn";
        base.type = MonsterType.Strong;

        base.attackTimer = .0f;

        base.projectileCNT = 5;
        base.projecttileAttackSpeed = 2.0f;
        base.projectAttackTimer = .0f;
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
