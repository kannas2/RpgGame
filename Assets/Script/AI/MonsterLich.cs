using UnityEngine;
using System.Collections;

public class MonsterLich : Monster
{
    void Start()
    {
        base.curHP = 1800.0f;
        base.maxHP = 1800.0f;

        base.minDamage = 45;
        base.maxDamage = 55;

        base.minSkllDamage = 65;
        base.maxSkillDamage = 75;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 2.0f;
        base.curMoveSpeed = 2.0f;

        base.rotSpeed = 100.0f;

        base.chaseTime = .0f;
        base.chaseCancleTime = 20.0f;

        base.preMonsterPos = transform.position;

        base.attackDis = 2.1f; //나중에 보고 조절할것.
        base.checkDis = 5.0f;
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        base.attack = false;  //이거 나주엥 손볼것 보스몬스터는 true상태로해서 선공.
        base.monsterName.text = "데모너스";
        base.itemPath = "Prefab/Item/soul";
        base.itemName = "soul";
        base.projectilePath = "Prefab/PoisonBullet";
        base.type = MonsterType.Boss;

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
        base.healCoolTime = .0f;

        base.attackState = true;
        base.prevattackState = true;
    }

    void FixedUpdate()
    {
        state.Update();
        base.MonsterUpdateHP();
        transform.GetComponent<CapsuleCollider>().enabled = !base.IsDead;   // 드래곤이랑 리치만 이렇게.
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
