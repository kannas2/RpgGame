using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float monster_HP = 100.0f;
    public float current_HP = 100.0f;

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
            Debug.Log("타겟");
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
            player.Character_Update_Hp(0.1f);
            player.charAnimation.SetBool("isDamage", true); //일단 이런식으로 구현하고 나중에 애니메이션 함수를 플레이어 클래스에서 한개 만들고 그 클래스에서 애니메이션 처리하는것으로 지금은 급한대로.

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