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

    public void BulletGetComponent()
    {
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
            Debug.Log("펑");
            Destroy(transform.gameObject, 1.0f);
        }
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