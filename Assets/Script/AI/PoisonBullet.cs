using UnityEngine;
using System.Collections;

public class PoisonBullet : Bullet {

    void Start()
    {
        base.boomDis = 0.5f;
        base.bulletSpeed = 5.0f;
        base.damage = 100;

        base.BulletGetComponent();

        base.boomPaticlePath = "Prefab/GroundExplode";
    }

    void FixedUpdate()
    {
        base.BulletMove();
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Player"))
        {
            PlayerCtrl.instance.OnDamage(damage);
            AttackPlayer();
        }
    }
}
