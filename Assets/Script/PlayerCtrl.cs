using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField]
    private VirtualJoystick joystick;

    public static PlayerCtrl instance;

    public Image hp_Image;
    public Image mp_Image;

    public BoxCollider attackBox;
    public Transform buffEffectpos;

    public CharacterState charstate;

    public Animator anim { get; set; }
    public bool attackChk { get; set; }

    void Awake()
    {
        instance = this;
        anim = transform.GetComponent<Animator>();
        attackBox = attackBox.GetComponent<BoxCollider>();
    }

    void Start()
    {
        base.playerName = "fabian";
        base.curExp = .0f;
        base.maxExp = .0f; // 기획서에 경험치 관련된건 없지만 .. 혹시나 해서 .

        base.curHP = 100.0f;
        base.maxHP = 100.0f;

        base.curMP = 100.0f;
        base.maxMP = 100.0f;

        base.baseAttackPower = 5;
        base.curAttackPower = 5;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 5.0f;
        base.curMoveSpeed = 5.0f;

        base.rotSpeed = 100.0f;
        base.isDead = false;

        charstate = CharacterState.Idle;
        attackChk = false;
        base.run = false;
    }

    void FixedUpdate()
    {
        Character_Move();
        Character_Anim_Speed();
        Character_Update_State();
        attackBox.enabled = attackChk;
    }

    protected override void Character_Move()
    {
        float vertical = joystick.Vertical();
        float horizontal = joystick.Horizontal() * base.rotSpeed;

        if (vertical >= 0)
            vertical *= base.curMoveSpeed;
        else
            vertical *= (base.curMoveSpeed * 0.5f);

        vertical *= Time.deltaTime;
        horizontal *= Time.deltaTime;

        transform.Translate(0, 0, vertical);
        transform.Rotate(0, horizontal, 0);

        if (vertical != 0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        //헤이스트 상태.
        anim.SetBool("Run", run);
    }

    public override void Character_Update_State()
    {
        hp_Image.fillAmount = curHP / maxHP;
        mp_Image.fillAmount = curMP / maxMP;

        if (curHP <= 0)
        {
            curHP = 0;
        }
        else if (curHP >= maxHP)
        {
            curHP = maxHP;
        }

        if (curMP <= 0)
        {
            curMP = 0;
        }
        else if (curMP >= maxMP)
        {
            curMP = maxMP;
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster") //이거 나중에 수정할것. 몬스터가 여러마리 될경우 충돌된 몬스터  HP를 까야함.
        {
            monster.curHP -= curAttackPower; //공격스킬 마다 어택 데미지가 달라짐.

            GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/Attack")) as GameObject; //공격 스킬마다 파티클 달라지게 할 예정.
            obj.transform.parent = other.transform;

            obj.transform.localPosition = other.transform.localPosition;
            Destroy(obj, 0.7f);
        }

        //아이템 드랍 .
        if(other.gameObject.tag == "HP_portion")
        {
            Inventory.instance.addItem(1);
            Destroy(other.gameObject);
        }
    }
    */
    //동작 스피드 컨트롤. anim.Play("animname",speed,streamtime);  //지금 임의로 애니메이터에서 속도 설정해놨음. 나중에 수정할것
    public override void Character_Anim_Speed()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        //이동 
        if (stateInfo.IsName("Walk"))
        {
            anim.speed = baseMoveSpeed;
        }
        else if (stateInfo.IsName("Run"))
        {
            anim.speed = 2.0f;
        }

        //기본상태
        if (stateInfo.IsName("Idle"))
        {
            anim.speed = 1.0f;
        }

        //공격
        if (stateInfo.IsName("Attack01"))
        {
            anim.speed = AttackSkill.instance.attackSpeed;
        }

        if (stateInfo.IsName("Brandish"))
        {
            anim.speed = Brandish.instance.attackSpeed;
        }
    }

    public void OnDamage(int damage)
    {
        curHP -= damage;

        GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/MonsterAttack")) as GameObject;
        //anim.SetTrigger("TakeDamage");
        anim.Play("TakeDamage");
        obj.transform.position = transform.position;
        Destroy(obj, 0.5f);
    }

    void OnTriggerEnter(Collider coll)
    {
        //몬스터에게 맞은경우. 지금 시간이 없어서 이런식으로 하는데 나중에는 아이템을 획득시 인벤토리 쪽으로 보내서 어떤 아이템인가 확인후 추가하는 방식으로.
        if(coll.gameObject.CompareTag("Item"))
        {
            switch(coll.gameObject.transform.name)
            {
                case "medalA":
                    {
                        coll.GetComponent<BoxCollider>().enabled = false;
                        Inventory.instance.addItem(1);
                        Destroy(coll.gameObject);
                    }
                    break;

                case "medalB":
                    {
                        coll.GetComponent<BoxCollider>().enabled = false;
                        Inventory.instance.addItem(2);
                        Destroy(coll.gameObject);
                    }
                    break;
                    
                case "medalC":
                    {
                        coll.GetComponent<BoxCollider>().enabled = false;
                        Inventory.instance.addItem(3);
                        Destroy(coll.gameObject);
                    }
                    break;

                case "dragonHorn":
                    {
                        coll.GetComponent<BoxCollider>().enabled = false;
                        Inventory.instance.addItem(4);
                        Destroy(coll.gameObject);
                    }
                    break;

                case "soul":
                    {
                        coll.GetComponent<BoxCollider>().enabled = false;
                        Inventory.instance.addItem(5);
                        Destroy(coll.gameObject);
                    }
                    break;

                default:
                    Debug.Log("해당 아이템이 없습니다.");
                    break;
            }

            ////캐릭터 체력 -
            ////파티클 생성
            //curHP -= 5.0f; //나중에 충돌체 몬스터의 공격데미지를 얻어와서 깔꺼임.
            //anim.SetTrigger("TakeDamage");
            //GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/MonsterAttack")) as GameObject;
            //obj.transform.position = transform.position;
            //Destroy(obj, 0.5f);
        }
    }

    //collider를 몇번 껏다 킬껀지 공격할때 Enter로 충돌 판정을 하는데 Enter는 처음에만 충돌 체크를 하기 떄문에.
    public IEnumerator AttackCount(int cnt)
    {
        for(int num=0; num< cnt; num++)
        {
            attackChk = true;
            yield return new WaitForSeconds(0.2f);
            attackChk = false;
        }
    }

    //void OnTriggerExit()
    //{
    //    attackChk = false;
    //}
}