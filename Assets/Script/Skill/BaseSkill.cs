using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseSkill : MonoBehaviour {

    //캐릭터 스킬에 필요한 것들...
    //쿨타임, 버프 시간, 버프 맥스시간,버프(힐, 이동속도), 공격속도, 데미지,  
    public float buffTime;
    public float coolTime;
    public float curCoolTime;
    public float useMp;

    public float healValue;
    public float speedValue;
    public bool  skillState;
    public Image button;
    public Text  time;

    public float attackSpeed;

    public int minDamage;
    public int maxDamage;

    public Transform effectPos;
    protected string effectpath;

    public PlayerCtrl player;

    public virtual void Heal() { }
    public virtual void Attack() { }
    public virtual void Buff() { }

    public virtual IEnumerator Buffwait(string buff,float ftime)
    {
        switch (buff)
        {
            case "Hast":
                {
                    SmallHast.instance.buffTime = 30.0f;
                    player.curMoveSpeed = 6.0f;
                    player.run = true;
                    yield return new WaitForSeconds(ftime);
                    player.run = false;
                    player.curMoveSpeed = player.baseMoveSpeed;
                }
                break;
        }
    }
    
    public void PaticleEffect()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load(effectpath)) as GameObject; //공격 스킬마다 파티클 달라지게 할 예정.
        obj.transform.parent = effectPos.transform;

        obj.transform.localPosition = effectPos.transform.localPosition;
        Destroy(obj, 0.7f);
    }

    public void CoolTime(Text time)
    {
        //쿨타임이 60초인데 0보다 작거나 같아지면 스킬을 사용할수 있게 설정.
        if (curCoolTime <= .0f)
        {
            skillState = true;
            curCoolTime = .0f;
            time.text = null;
        }
        else if (curCoolTime >= .0f)// 0보다 클경우 값을 1초씩 감산. 스킬을 사용못함.
        {
            curCoolTime -= 1.0f * Time.deltaTime;
            skillState = false;
            int value = (int) curCoolTime;
            time.text = value.ToString();
        }
    }

    public void GetComponent()
    {
        button = transform.GetComponent<Image>();
        time = transform.FindChild("Time").GetComponent<Text>();
        player = PlayerCtrl.instance;
    }

    public int RandomDamage(int min, int max)
    {
        int rand = Random.Range(min, max);
        return rand;
    }
}
