using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField]
    private VirtualJoystick joystick;
    [SerializeField]
    private Inventory inventory;

    public  Transform      hp_Image;
    private Monster        monster;
    public  Collider       attackBox;

    public CharacterState charstate;

    public Animator charAnimation { get; set; }
    public bool isDead           { get; set; }
    public bool attackChk        { get; set; }

    private BaseCharacter player;

    void Awake()
    {
        //                              name, exp, speed, rotspeed, hp, mp
        player = new BaseCharacter("player", .0f, 3.0f, 100.0f, 100.0f, 100.0f);
             
        charAnimation = transform.FindChild("Query-Chan-SD_Black").FindChild("SD_QUERY_02").GetComponent<Animator>();
        monster      = GameObject.Find("Monster").GetComponent<Monster>();
        attackBox    = attackBox.GetComponent<MeshCollider>();
    }

    void Start()
    {
        charstate = CharacterState.Idle;
        isDead = false;
        attackChk = false;
    }

    void FixedUpdate()
    {
        Character_Move();
        attackBox.enabled = attackChk;
    }

    protected override void Character_Move()
    {
        float vertical = joystick.Vertical();
        float horizontal = joystick.Horizontal() * player.characterRotSpeed;

        if (vertical >= 0)
            vertical *= player.characterSpeed;
        else
            vertical *= (player.characterSpeed * 0.5f);

        vertical *= Time.deltaTime;
        horizontal *= Time.deltaTime;

        transform.Translate(0, 0, vertical);
        transform.Rotate(0, horizontal, 0);

        if (vertical != 0)
        {
            charAnimation.SetBool("isWalking", true);
        }
        else
        {
            charAnimation.SetBool("isWalking", false);
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
        if (other.gameObject.tag == "Monster")
        {
            monster.current_HP -= 10.0f;

            GameObject obj = (GameObject)Instantiate(Resources.Load("Particle/Attack")) as GameObject;
            obj.transform.parent = other.transform;

            obj.transform.localPosition = other.transform.localPosition;
            Destroy(obj, 0.7f);
        }

        if(other.gameObject.tag == "HP_portion")
        {
            inventory.addItem(1);
            Destroy(other.gameObject);
        }
    }
}