using UnityEngine;
using System.Collections;

public class MonsterAngel : Monster {

	void Start ()
    {
        base.curHP = 50.0f;
        base.maxHP = 50.0f;

        base.startDamage = 10.0f;
        base.maxDamage   = 15.0f;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 5.0f;

        base.attackState = false;
        base.preMonsterPos = transform.position;
        base.curMonsterPos = transform;

        base.attackDis = 0.9f;
        base.checkDis = 2.5f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.monsterName.text = "아가사";
        base.itemPath = "Prefab/DropItem";

    }
	
	void FixedUpdate()
    {
        state.Update();
        //base.MonsterUpdateHP();
	}

    //물리적 충돌X
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            StartCoroutine(base.AttackCheckTime());
        }
    }
}
