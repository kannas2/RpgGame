using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField]
    private VirtualJoystick joystick;

    public static PlayerCtrl instance; 

    public  Transform      hp_Image;
    private Monster        monster;
    public  Collider       attackBox;

    public CharacterState charstate;

    public Animator anim   { get; set; }
    public bool attackChk  { get; set; }

    void Awake()
    {
        instance = this;
        anim = transform.FindChild("Query-Chan-SD_Black").FindChild("SD_QUERY_02").GetComponent<Animator>();
        monster      = GameObject.Find("Monster").GetComponent<Monster>();
        attackBox    = attackBox.GetComponent<MeshCollider>();
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

        base.baseMoveSpeed = 1.0f;
        base.curMoveSpeed = 1.0f;

        base.rotSpeed = 100.0f;
        base.isDead = false;

        charstate = CharacterState.Idle;
        attackChk = false;
    }

    void FixedUpdate()
    {
        Character_Move();
        Character_Anim_Speed();
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
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public override void Character_Update_Hp(float damage)
    {
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
            anim.speed = curMoveSpeed;
        }
        else if (stateInfo.IsName("Attack"))
        {
            anim.speed = curAttackSpeed;
        }
        else
        {
            anim.speed = 1.0f;
        }
    }
}