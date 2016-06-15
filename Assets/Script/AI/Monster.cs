using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public enum MonsterType
    {
        Weak,
        Strong,
        Boss,
    }

    public float maxHP;
    public float curHP;

    public float maxMP;
    public float curMP;

    public int maxDamage;
    public int minDamage;

    //ㅡㅡㅡㅡ정예, 보스 한테만 사용될 변수 ㅡㅡㅡㅡ

    public int minSkllDamage;
    public int maxSkillDamage;

    public int   projectileCNT;         //날릴 구체 개수.
    public float projectAttackTimer;     //원거리공격. 
    public float projecttileAttackSpeed;  //구체 생성 시간 간격.
    public float projecttileCoolTime;
    public string projectilePath;
    public float healCoolTime;

    public Transform ProjectilePos;       //발사 지점.
    public Vector3[] pivotPos;

   //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public float attackDis;
    public float checkDis;

    public float baseAttackSpeed;
    public float curAttackSpeed;

    public float baseMoveSpeed;
    public float curMoveSpeed;

    public float rotSpeed;

    public float chaseTime;
    public float chaseCancleTime;

    public bool IsDead; //false;

    public Transform myTarget { get; set; }
    public PlayerCtrl player;

    public Vector3 preMonsterPos; //몬스터 시작지점.
    public Vector3 prevRot;      //몬스터가 원래 보고 있던 각도.

    public State_Machine<Monster> state;

    //------------------------
    public Image imageHPbar;
    public Image imageMPbar;

    public Text monsterName;
    public Text takeDamage;
    public Animator anim;
    public string itemPath;  //아이템 경로.

    public bool attackState;       //선공 여부.
    public bool prevattackState;

    public SphereCollider[] checkbox;
    public bool attack; //공격이 가능한 상태인지.
    public string itemName;

    public float attackTimer;
    public Rigidbody body;

    public MonsterType type;
    public float dieTime;
    public AnimatorStateInfo stateInfo;

    public virtual void GetComponent()
    {
        myTarget = PlayerCtrl.instance.transform;
        player = PlayerCtrl.instance;
        anim = transform.GetComponent<Animator>();
        //checkbox = transform.GetComponent<SphereCollider>(); //드래그 해서 직접 넣었음.
        body = transform.GetComponent<Rigidbody>();
        monsterName = transform.FindChild("UI").Find("Name").GetComponent<Text>();
        takeDamage = transform.FindChild("UI").Find("Damage").GetComponent<Text>();

        if (state == null)
        {
            state = new State_Machine<Monster>();
        }
    }

    public void ResetState()
    {
        curHP = maxHP;
        attackTimer = .0f;
        projecttileCoolTime = .0f;
        attackState = prevattackState;

        myTarget = player.transform;
        transform.rotation = Quaternion.Euler(prevRot);
        state.Initial_Setting(this, State_Idle.Instance);
    }

    //상태변경
    public void ChangeState(FSM_State<Monster> _state)
    {
        state.ChangeState(_state);
    }

    //회전 각도 체크.
    public bool Check_Angle()
    {
        if (Vector3.Dot(myTarget.position, transform.position) >= 1.5f)
        {
            return true;
        }
        return false;
    }

    //일정 거리내에 있는지 체크.
    public float CheckRange()
    {
        float dis = Vector3.Distance(myTarget.position, transform.position);

        return dis;
    }

    public void MonsterUpdateHP()
    {
        imageHPbar.fillAmount = curHP / maxHP;

        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
        else if (curHP <= 0)
        {
            curHP = 0;
        }

        //MP가 있는 몬스터일 경우? 모르겠음 기획서에는 없음.
        if (imageMPbar != null)
        {
            imageMPbar.fillAmount = curMP / maxMP;
            if (curMP >= maxMP)
            {
                curMP = maxMP;
            }
            else if (curMP <= 0)
            {
                curMP = 0;
            }
        }
    }

    public void MonsterMove(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        Vector3 nordir = dir.normalized;

        Quaternion angle = Quaternion.LookRotation(nordir);
        transform.rotation = angle;

        Vector3 pos = transform.position;
        pos += transform.forward * Time.smoothDeltaTime * curMoveSpeed;
        transform.position = pos;
    }

    public void MonsterMoveReset()
    {

        float distance = Vector3.Distance(preMonsterPos, transform.position);
        if (distance >= 0.1f)
        {
            MonsterMove(preMonsterPos);
        }
        else
        {
            ResetState();
        }
    }

    public void MonsterDie()
    {
        Destroy(transform.gameObject);
    }

    public void PlayerLook()
    {
        Vector3 dir = (myTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * curMoveSpeed);
    }

    public void Create_item()
    {
        GameObject item = (GameObject)Instantiate(Resources.Load(itemPath), transform.position, Quaternion.identity);
        item.transform.position = new Vector3(transform.position.x, transform.position.y + 6.0f, transform.position.z);
        item.name = itemName;
    }

    public void DamageAni(string text)
    {
        GameObject damage = Instantiate(Resources.Load("Prefab/Damage")) as GameObject;
        RectTransform rect = damage.GetComponent<RectTransform>();
        damage.transform.SetParent(transform.Find("UI"));

        rect.transform.localPosition = monsterName.transform.localPosition;
        rect.transform.localScale = monsterName.transform.localScale;

        Text damageValue = damage.GetComponent<Text>();
        damageValue.text = text;

        damage.GetComponent<Animator>().SetTrigger("Hit");
        Destroy(damage, 1.0f);
    }

    //몬스터가 Attack일 경우에만 사용되는 함수.
    public void MosnterHeal()
    {
        //모션 전환
        anim.SetTrigger("Heal");
        //파티클 생성
        GameObject heal = Instantiate(Resources.Load("Prefab/Skill/LifeEnchant")) as GameObject;
        heal.transform.position = transform.position;
        //몬스터 체력 회복
        curHP += 100;
        //파티클 삭제
        Destroy(heal.gameObject, 1.5f);
    }

    public void CreateBullet(string bulletPath, float sec)
    {
        StartCoroutine(BulletAttack(bulletPath, sec));
    }

    public IEnumerator BulletAttack(string bulletPath,float sec)
    {
        yield return new WaitForSeconds(sec);
        //나중에 파이어볼 생성하면서 플레이어 위치 발? 부분에 빨간색 발판? 같은 느낌 같은거 생성되게끔.
        GameObject bullet = (GameObject)Instantiate(Resources.Load(bulletPath)) as GameObject;
        bullet.transform.position = CurrentPos();
    }

    //이 함수를 만든 이유는 Trasnform을 얻어오면 Transform 같은경우는 동적으로 계속 변경되는데
    //Vector3는 변동이 되지 않고 그때 당시 딱 그 위치만을 대입받기 때문에.
    //이것은 예전 몬스터가 공격을 못할경우 이전 위치로 되돌아가는데 그 되돌아가야 할 위치를
    //Transform 으로 받았는데 돌아갈 위치가 자꾸 변동되는것을 보고 그때 알게 된.
    public Vector3 CurrentPos()
    {
        Vector3 currentPos = ProjectilePos.position ;

        return currentPos;
    }

    public void OnDamage(float damage)
    {
        curHP -= damage;
        //anim.SetTrigger("takeDamage"); 공격 state 에서 onDamage가 true라면 공격모션대신 피격 모션 실행하고 false 하는 방식으로? 
        GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/BaseAttack")) as GameObject;
        obj.transform.position = transform.position;
        Destroy(obj, 0.5f);

        int value = (int)damage; 
        DamageAni(value.ToString());
    }

    public int RandomDamage(int min, int max)
    {
        int rand = Random.Range(min, max);
        return rand;
    }

    public void TriggerEnter(Collider coll)
    {
        //검에 맞았을경우.
        if (coll.gameObject.CompareTag("Sword"))
        {
            //타격 이펙트 생성,
            attackState = true;
            OnDamage(player.curAttackPower);
        }

        if (coll.gameObject.CompareTag("Player") && attack) //어택스테이트에서 attack true해주면 될듯. 초반에.
        {
            attack = false;
            player.OnDamage(RandomDamage(minDamage, maxDamage));
            //StartCoroutine(AttackCheckTime());
        }
    }

    /*
       //몬스터가 공격받은 현재의 반대 방향을 구해야함. 근데 몬스터 특성상 그냥 서있는 상태의 반대방향만 구하면 될거같..
       //Vector3 dir = myTarget.position - curMonsterPos.position;
       //dir = dir.normalized;

       //body.AddForce(-dir * 10, ForceMode.Impulse);
       //Debug.Log("충돌함");
       */
}
