using UnityEngine;
using System.Collections;

public class FireBullet : Bullet
{
    void Start()
    {
        base.boomDis = 0.5f;
        base.bulletSpeed = 5.0f;
        base.BulletGetComponent();

        base.boomPaticlePath = "Prefab/GroundExplode";
    }

    void Update()
    {
        base.BulletMove();
    }
}
