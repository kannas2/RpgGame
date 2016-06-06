using UnityEngine;
using System.Collections;

public class MonsterBunny : Monster {

	void Start ()
    {
        base.curHP = 150.0f;
        base.maxHP = 150.0f;

        base.startDamage = 20.0f;
        base.maxDamage = 25.0f;

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

        base.attackDis = 0.9f; //플레이어 공격 거리  
        base.checkDis = 2.5f;  //플레이어 인식 거리. 
        base.IsDead = false;

        base.GetComponent();
        base.ResetState();

        //Getcomponent이후.
        base.monsterName.text = "래피";
        base.itemPath = "Prefab/DropItem";
    }
	
	void FixedUpdate()
    {
        //curMonsterPos = transform; //몬스터의 현재 위치.
        state.Update();
        //base.MonsterUpdateHP();
	}

    //물리적 충돌X
    void OnTriggerEnter(Collider coll)
    {
        //검에 맞았을경우.
        if (coll.gameObject.CompareTag("Sword"))
        {
            //타격 이펙트 생성,
            base.OnDamage();
        }

        //if(coll.gameObject.CompareTag("Player"))
        //{ 
        //    StartCoroutine(base.AttackCheckTime());
        //}

        //if(coll.gameObject.CompareTag("Sword"))
        //{
        //    base.OnDamage(coll.)
        //}
    }
}