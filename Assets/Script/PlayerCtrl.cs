using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField]
    private VirtualJoystick joystick;

    public static PlayerCtrl instance;
    public ItemDatabase itemData;

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
        itemData = ItemDatabase.Instance;
        Debug.Log("itemData : " + itemData);
        anim = transform.GetComponent<Animator>();
        attackBox = attackBox.GetComponent<BoxCollider>();
    }

    void Start()
    {
        base.playerName = "fabien";
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

    //동작 스피드 컨트롤. anim.Play("animname",speed,streamtime);
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

    //기존 스위치문을 축소함. 나중에 for문이 아닌 그냥 아이템 먹으면 add코드가 바로 가능하게끔 해보기.
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Item"))
        {
            for (int i = 0; i < itemData.items.Count; i++)
            {
                if (itemData.items[i].itemName == coll.gameObject.name)
                {
                    coll.GetComponent<BoxCollider>().enabled = false;
                    Inventory.instance.addItem(i + 1);
                    Destroy(coll.gameObject);

                    //나중에 수정할것. 지금 당장 방법이 없음..
                    switch (itemData.items[i].itemID)
                    {
                        case 1:
                            if (NpcDick.instance.currentValue < NpcDick.instance.qwestValue)
                                NpcDick.instance.currentValue += 1;

                            UIControl.Instance.screenText.Add(" 메달 A "+NpcDick.instance.currentValue + "/" + NpcDick.instance.qwestValue);
                            break;

                        case 2:
                            if (NpcDilliseu.instance.currentValue < NpcDilliseu.instance.qwestValue)
                                 NpcDilliseu.instance.currentValue += 1;

                            UIControl.Instance.screenText.Add(" 메달 B " + NpcDilliseu.instance.currentValue + "/" + NpcDilliseu.instance.qwestValue);
                            break;

                        case 3:
                            if (NpcChase.instance.currentValue < NpcChase.instance.qwestValue)
                                NpcChase.instance.currentValue += 1;

                            UIControl.Instance.screenText.Add(" 메달 C " + NpcChase.instance.currentValue + "/" + NpcChase.instance.qwestValue);
                            break;

                        case 4:
                            if (NpcWilter.instance.currentValue < NpcWilter.instance.qwestValue)
                                NpcWilter.instance.currentValue += 1;

                            UIControl.Instance.screenText.Add(" 드래곤의 뿔 " + NpcWilter.instance.currentValue + "/" + NpcWilter.instance.qwestValue);
                            break;

                        case 5:
                            if (NpcFidelio.instance.currentValue < NpcFidelio.instance.qwestValue)
                                NpcFidelio.instance.currentValue += 1;

                            UIControl.Instance.screenText.Add(" 데모너스의 구슬 " + NpcFidelio.instance.currentValue + "/" + NpcFidelio.instance.qwestValue);
                            break;
                    }
                }
            }
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