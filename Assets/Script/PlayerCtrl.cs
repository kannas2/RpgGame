using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField]
    private VirtualJoystick joystick;

    public static PlayerCtrl instance; 

    public  Image         hp_Image;
    private Monster        monster;
    public  BoxCollider    attackBox;

    public CharacterState charstate;

    public Animator anim   { get; set; }
    public bool attackChk  { get; set; }

    void Awake()
    {
        instance = this;
        anim = transform.GetComponent<Animator>();
        monster      = GameObject.Find("Monster").GetComponent<Monster>();
        attackBox    = attackBox.GetComponent<BoxCollider>();
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
    }

    void FixedUpdate()
    {
        Character_Move();
        Character_Anim_Speed();
        Character_Update_Hp();
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

        //키보드라면 뭐 시프트를 누른상태면 Walk가 Run으로 변경되게 설정을 할수 있겠는데
        //모바일이라서.. 걷기/뛰기 동작 둘중 하나만 설정함.
        if (vertical != 0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);
    }

    public override void Character_Update_Hp()
    {
        hp_Image.fillAmount = curHP / maxHP;

        if(curHP <= 0)
        {
            curHP = 0;
            Debug.Log("게임오버");
        }
        else if(curHP >= maxHP)
        {
            curHP = maxHP;
        }

        /* 서서히 피 달게 하는 방법도 있는데 음.
        if (hp_Image.transform.localScale.y >= 0)
        {
            float y_size;
            hp_Image.transform.localScale -= new Vector3(.0f, damage, .0f);

            y_size = -259 - (100.0f * 0.5f * (1 - hp_Image.localScale.y));
            hp_Image.transform.localPosition = new Vector3(.0f, y_size, .0f);
        }
        else
        {
            damage = 0;
            hp_Image.transform.localScale = new Vector3(.0f, .0f, .0f);
            //Application.LoadLevel(5);
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster") //이거 나중에 수정할것. 몬스터가 여러마리 될경우 충돌된 몬스터  HP를 까야함.
        {
            monster.current_HP -= 10.0f;

            GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/Attack")) as GameObject;
            obj.transform.parent = other.transform;

            obj.transform.localPosition = other.transform.localPosition;
            Destroy(obj, 0.7f);
        }

        if(other.gameObject.tag == "HP_portion")
        {
            Inventory.instance.addItem(1);
            Destroy(other.gameObject);
        }
    }

    //동작 스피드 컨트롤.
    public override void Character_Anim_Speed()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Walk"))
        {
            anim.speed = curMoveSpeed-1;
        }
        else if (stateInfo.IsName("Attack01"))
        {
            anim.speed = curAttackSpeed;
        }
    }
}