using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QwestManager : Singleton<QwestManager>
{
    public Image      qwestContent;
    public GameObject  parent;
    public GameObject  QwestScreen;

    public Dictionary<string, bool> qwest = new Dictionary<string, bool>();

    //퀘스트 완료 여부를 알기 위해서 만든 딕셔너리.


    //캐릭터가 현재 진행중인 퀘스트가 무엇인지 받아올것.
    //특정 퀘스트진행시 조건문을 통해 퀘스트를 클리어 해야지만 다음 퀘스트가 열리게 끔 할것.
    //퀘스트 완료 여부를 컨트롤 해주어야함.
    //딕셔너리를 이용하여 해당 퀘스트가 클리어 된것만 퀘스트 완료 마크가 찍히게끔 설정.

    void Start()
    {
        CreateQwest("ms101");
    }

    void FixedUpdate()
    {
    }

    public void ActiveCompletMark()
    {
        foreach (string str in qwest.Keys)
        {
            if (qwest[str] == true)
            {
                parent.transform.FindChild(str).FindChild("Complete_s").GetComponent<Image>().enabled = true;
            }
        }
    }

    //퀘스트 목록 생성.
    public void CreateQwest(string qwestcode)
    {
        Sprite image = (Sprite)Resources.Load("GameUI/" + qwestcode, typeof(Sprite));
        GameObject qwest = Instantiate(Resources.Load("Prefab/Qwest")) as GameObject;
        qwest.transform.SetParent(parent.transform);
        qwest.name = qwestcode;
        qwest.GetComponent<Image>().sprite = image;

        RectTransform info = qwest.GetComponent<RectTransform>();
        info.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        info.localPosition = new Vector3(info.localPosition.x, info.localPosition.y, .0f);
    }

    //망함. 그냥 클릭했을때 오른쪽 퀘스트 내용 변경해주는 형식으로 사용하자.
    public void CompleteQwest(string qwestcode) //클릭한거 기준으로 확인여부.
    {
        bool key = qwest.ContainsKey(qwestcode);
        if (key != false)
        {
            qwestContent.transform.FindChild("Complete_b").GetComponent<Image>().enabled = qwest[qwestcode];
        }
        Sprite image = (Sprite)Resources.Load("GameUI/Content/" + qwestcode, typeof(Sprite));
        qwestContent.GetComponent<Image>().sprite = image;
    }

    //퀘스트창 킬때와 끌때.
    public void QwestListInit()
    {
        CompleteQwest("ms101"); //초기화면
        ActiveCompletMark();
    }

    //public bool QwestAddCheck(string key)
    //{
    //    foreach (string str in qwest.Keys)
    //    {
    //        Debug.Log("코드확인");
    //        if (qwest.ContainsKey(key))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}

