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
    private Monster monster;
    public BoxCollider attackBox;
    public Transform buffEffectpos;

    public CharacterState charstate;

    public Animator anim { get; set; }
    public bool attackChk { get; set; }

    void Awake()
    {
        instance = this;
        anim = transform.GetComponent<Animator>();
        //monster      = GameObject.Find("Monster").GetComponent<Monster>();
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

        base.baseAttackPower = 1.0f;
        base.curAttackPower = 1.0f;

        base.baseAttackSpeed = 1.0f;
        base.curAttackSpeed = 1.0f;

        base.baseMoveSpeed = 3.0f;
        base.curMoveSpeed = 3.0f;

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
            Debug.Log("게임오버");
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
            anim.speed = curAttackSpeed;
        }

        if (stateInfo.IsName("Brandish"))
        {
            anim.speed = Brandish.instance.attackSpeed;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        //몬스터에게 맞은경우.
        if(coll.gameObject.CompareTag("Monster"))
        {
            //캐릭터 체력 -
            //파티클 생성
            curHP -= 5.0f; //나중에 충돌체 몬스터의 공격데미지를 얻어와서 깔꺼임.
            anim.SetTrigger("TakeDamage");
            GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/MonsterAttack")) as GameObject;
            obj.transform.position = transform.position;
            Destroy(obj, 0.5f);
        }
    }

    void OnTriggerExit()
    {
        attackChk = false;
    }
}