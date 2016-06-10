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
    public float projecttileAttackSpeed; //구체 생성 시간 간격.

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
    public Transform curMonsterPos; //현재 위치 이동해서의.
    public Vector3 prevRot;      //몬스터가 원래 보고 있던 각도.

    public int monsterPatten; //몬스 특정 패턴.

    public State_Machine<Monster> state;

    //------------------------
    public Image imageHPbar;
    public Image imageMPbar;

    public Text monsterName;
    public Text takeDamage;
    public Animator anim;
    public string itemPath;  //아이템 경로.

    public bool attackState;       //선공 여부.
    public SphereCollider checkbox;
    public bool attack; //공격이 가능한 상태인지.
    public string itemName;

    public float attackTimer;
    public Rigidbody body;

    public MonsterType type;

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
        myTarget = player.transform;
        curMonsterPos.rotation = Quaternion.Euler(prevRot);
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
        if (Vector3.Dot(myTarget.position, curMonsterPos.position) >= 1.5f)
        {
            return true;
        }
        return false;
    }

    //일정 거리내에 있는지 체크.
    public float CheckRange()
    {
        float dis = Vector3.Distance(myTarget.position, curMonsterPos.position);

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

    public void PlayerLook()
    {
        Vector3 dir = (myTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * curMoveSpeed);
    }

    public void Create_item()
    {
        GameObject item = (GameObject)Instantiate(Resources.Load(itemPath), curMonsterPos.position, Quaternion.identity);
        item.transform.position = new Vector3(curMonsterPos.position.x, curMonsterPos.position.y + 5.0f, curMonsterPos.position.z);
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

    //attack box on/off; 사용 안할 예정 삭제 예정.
    public IEnumerator AttackCheckTime()
    {
        attack = false;
        yield return new WaitForSeconds(curAttackSpeed);
        attack = true;
    }

    public void OnDamage(float damage)
    {
        Debug.Log("맞음");
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
            OnDamage(player.curAttackPower);
        }

        if (coll.gameObject.CompareTag("Player")&& attack) //어택스테이트에서 attack true해주면 될듯. 초반에.
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
/*
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float max_HP = 100.0f;
    public float cur_HP = 100.0f;

    private float attack_damage = 1.0f;

    public float Attack_Range = 2.5f;
    public float attack_speed = 1.5f;
    public float move_speed = 2.5f;
    public float rot_speed = 100f;

    public Animation ani = null;
    public State_Machine<Monster> state = null;

    public float chase_time = .0f;
    public float chase_cancle_time = 5.0f;

    public bool IsDead = false;

    public Transform my_target { get; set; }
    public PlayerCtrl player { get; set; }

    public Vector3 set_monster_pos;
    public Vector3 prev_rot;

    public Slider HP_bar;

    private void Awake()
    {
        set_monster_pos = new Vector3(.0f, 1.1f, -2.72f);
        prev_rot = new Vector3(.0f, .0f, .0f);

        //애니메이션 컴포넌트 가져오기
        ani = this.GetComponent<Animation>();

        //초기설정
        ResetState();

        if (GameObject.Find("Player"))
        {
            my_target = GameObject.Find("Player").GetComponent<Transform>();
            player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        }
    }

    private void Update()
    {
        //상태 업데이트
        state.Update();
        //체력바 업데이트
        Monster_Update_HP();
    }

    void HP_Bar_Update()
    {

    }

    public void ResetState()
    {
        if (state == null)
        {
            state = new State_Machine<Monster>();
        }

        my_target = GameObject.Find("Player").GetComponent<Transform>();

        //몬스터 세팅된 포지션
        transform.position = set_monster_pos;
        transform.rotation = Quaternion.Euler(prev_rot);

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
        if (Vector3.Dot(my_target.transform.position, transform.position) >= 0.5f)
        {
            return true;
        }
        return false;
    }

    //일정 거리내에 있는지 체크.
    public bool Check_Range()
    {
        if (Vector3.Distance(my_target.transform.position, transform.position) <= Attack_Range)
        {
            return true;
        }
        return false;
    }

    public void Monster_Update_HP()
    {
        float fHp = (float)current_HP / (float)monster_HP;
        HP_bar.value = fHp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("충돌했습니다.");
            //player.Character_Update_Hp(0.1f);
            PlayerCtrl.instance.curHP -= 10.0f;
            player.anim.SetBool("isDamage", true); //일단 이런식으로 구현하고 나중에 애니메이션 함수를 플레이어 클래스에서 한개 만들고 그 클래스에서 애니메이션 처리하는것으로 지금은 급한대로.

            GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/Attack")) as GameObject;
            obj.transform.parent = other.transform;

            obj.transform.localPosition = other.transform.localPosition;
            Destroy(obj, 0.7f);
            Debug.Log("other : " + other.transform.localPosition);
        }
    }

    public void Create_item()
    {
        GameObject item = (GameObject)Instantiate(Resources.Load("Prefab/DropItem"), transform.position, Quaternion.identity);
        item.transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
    }
}
    */
