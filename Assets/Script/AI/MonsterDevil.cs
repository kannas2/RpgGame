using UnityEngine;
using System.Collections;

public class MonsterDevil : Monster {

    void Start()
    {
        base.curHP = 50.0f;
        base.maxHP = 50.0f;

        base.minDamage = 15;
        base.maxDamage = 20;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 2.0f;  //어택 딜레이타임이 더 맞는 말인듯.

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 5.0f;

        base.preMonsterPos = transform.position;

        base.attackDis = 0.9f;
        base.checkDis = 2.5f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.attackState = false;
        base.prevattackState = false;
        base.attack = false;

        base.monsterName.text = "길리";
        base.itemPath = "Prefab/Item/medalB";
        base.itemName = "medalB";

        base.attackTimer = .0f;
        base.dieTime = .0f;
    }

    void FixedUpdate()
    {
        if (attackState == true)
        {
            state.Update();
            base.MonsterUpdateHP();
        }
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
