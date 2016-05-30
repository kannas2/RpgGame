using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HasteBuff : BaseSkill {

    public static HasteBuff instance;

	void Start ()
    {
        instance = this;
        base.buffTime         = 30.0f;
        base.buffLimitTime     = .0f;
        base.buffCoolTime      = 60.0f;
        base.curbuffCoolTime   = 0.0f;
        base.speedValue       = 5.0f;
        base.buffState        = true;
        base.button = transform.GetComponent<Image>();
    }
	
	void FixedUpdate()
    {
        base.CoolTime();
        button.fillAmount = curbuffCoolTime / buffCoolTime;
        Debug.Log("캐릭터 이동속도 : " + PlayerCtrl.instance.curMoveSpeed);
	}

    public override void Buff()
    {
        //캐릭터 스테이스트 창 밑에 헤이스트 시간도 표기 해주어야함.
        //30초가 지나면 자동으로 삭제되어야함.
        curbuffCoolTime = 60.0f;
        StartCoroutine(base.Buffwait("Hast", buffTime));
    }

    //IEnotor?? 그거 사용하면 좀더 효율적이다 라고 하는데.. 나중에 변경하던가 ...
    //public override IEnumerator Buff()
    //{
    //    //캐릭터 스테이스트 창 밑에 헤이스트 시간도 표기 해주어야함.
    //    //30초가 지나면 자동으로 삭제되어야함.
    //    curbuffCoolTime = 60.0f;
    //    while (buffTime >= buffLimitTime)
    //    {
    //        Debug.Log("헤이스트 타임 : " + buffTime);
    //        //버프는 처음에 30초이며 0초까지 지속이 된다.
    //        buffTime -= 1.0f * Time.deltaTime;
    //        if (buffTime >= buffLimitTime)
    //        {
    //            //캐릭터의 이동속도를 5로 변경시켜주어야 한다.
    //            PlayerCtrl.instance.curMoveSpeed = 5.0f;
    //            //캐릭터 위에 헤이스트 이펙트 생성 후 삭제.
    //            //캐릭터에 모션있으면 모션설정도. 조이스틱버튼에서 설정.
    //        }
    //        else
    //        {
    //            buffTime = 30.0f;
    //            PlayerCtrl.instance.curMoveSpeed = PlayerCtrl.instance.baseMoveSpeed;
    //            break;
    //        }
    //    }
    //}
}