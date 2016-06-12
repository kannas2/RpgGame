using UnityEngine;
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
        base.chaseCancleTime = 20.0f;

        base.attackState = true;
        base.preMonsterPos = transform.position;

        base.attackDis = 3.1f; //나중에 보고 조절할것.
        base.checkDis = 5.0f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.attack = false;
        base.monsterName.text = "드 래 곤";
        base.itemPath = "Prefab/Item/dragonHorn";
        base.itemName = "dragonHorn";
        base.type = MonsterType.Strong;

        base.attackTimer = .0f;

        base.projectileCNT = 0;
        base.projecttileAttackSpeed = 3.0f;
        base.projectAttackTimer = .0f;

        base.pivotPos = new Vector3[] { new Vector3(7.454f, 2.154f, 33.4f),
                                     new Vector3(11.66f, 3.0f,  42.89f),
                                     new Vector3(20.66f, 2.04f, 35.23f)
                                   };
        base.dieTime = .0f;
        base.projecttileCoolTime = 20.0f;

    }
	
	void FixedUpdate()
    {
        state.Update();
        base.MonsterUpdateHP();
        transform.GetComponent<BoxCollider>().enabled = !base.IsDead;   // 드래곤이랑 리치만 이렇게.
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
