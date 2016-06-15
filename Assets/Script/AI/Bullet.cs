using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    protected Vector3 playerPos;

    public BoxCollider box;
    public Rigidbody body;

    public string boomPaticlePath;

    public float boomDis;
    public float bulletSpeed;
    public bool  boom;
    public float damage;

    public void BulletGetComponent()
    {
        boom = false;
        box = GetComponent<BoxCollider>();
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;

        playerPos = PlayerCtrl.instance.transform.position;
    }

    protected void BulletMove()
    {
        if (Dis(transform, playerPos) >= boomDis)
        {
            //이동
            transform.Translate(Dir(transform, playerPos) * (bulletSpeed * Time.deltaTime));
        }
        else
        {
            //펑
            if(boom == false)
            {
                boom = true;
                GameObject damage = Instantiate(Resources.Load(boomPaticlePath)) as GameObject;
                damage.transform.SetParent(transform);
                damage.transform.position = transform.position;
                ShakeCamera.instance.shakeDuration = 0.3f;
                Destroy(damage.gameObject, 3.0f);
                Destroy(transform.gameObject, 1.0f);
            }
        }
    }

    public void AttackPlayer()
    {
        GameObject damage = Instantiate(Resources.Load("Prefab/Skill/AirExplode")) as GameObject;
        damage.transform.SetParent(PlayerCtrl.instance.transform);
        ShakeCamera.instance.shakeDuration = 0.3f;
        Destroy(damage, 2.5f);
    }

    public float Dis(Transform bullet, Vector3 player)
    {
        float dis = Vector3.Distance(player, bullet.position);
        return dis;
    }

    public Vector3 Dir(Transform bullet, Vector3 player)
    {
        Vector3 dir = player - bullet.position;
        Vector3 nordir = dir.normalized;

        return nordir;
    }
}