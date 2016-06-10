using UnityEngine;
using System.Collections;

public class FireBullet : Bullet
{
    void Start()
    {
        base.boomDis = 0.5f;
        base.bulletSpeed = 1.0f;
        base.BulletGetComponent();

        base.boomPaticlePath = "";
    }

    void Update()
    {
        base.BulletMove();
    }
}
