using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using System;

[Serializable]
public class BaseNpc : MonoBehaviour
{
    public enum NpcType
    {
        Shop,
    }
    protected Transform target;
    protected CapsuleCollider coll;

    protected string    npcName;
    protected Vector3   npcPostion;
    public    Sprite    npcImage;

    public float     textSpeed;
    public int       textIndex;
    public int       currentIndex = 0;
    public string    currentqwest;
    public string    prevqwest;
    protected string limitedcode;
    public NpcType   npcType;

    public bool sceenCut      = false;
    public bool qwestUIState   = true;
    public string selectButton = null;
    public bool pick          = false;

    //퀘스트 나중에 몇마리 잡아오라 이런거 할때 사용될변수.
    public int qwestValue;
    public int currentValue;
    //그전 NPC의 퀘스트를 완료 했는지 여부 확인할 변수.
    public string prevCheckQwest;

    //캐릭터 애니메이션
    protected Animator ani;
    
    //쉐이더.
    protected Shader shader1;
    protected Shader shader2;
    protected SkinnedMeshRenderer mesh; //나중에 애니가 들어간 캐릭터 올때 SkinnedMeshRenderer로 교체할것.

    // npc대화 창 설정할  list
    public List<string> textStory  = new List<string>();
    public List<string> textName   = new List<string>();
    public List<string> code      = new List<string>();
    public List<string> point     = new List<string>();
    public List<string> character  = new List<string>();
    public List<string> select    = new List<string>();
    public List<string> jump      = new List<string>();
    public List<string> selectA   = new List<string>();
    public List<string> selectB   = new List<string>();
    public List<string> sceen     = new List<string>();
    public List<string> restart   = new List<string>();
    public List<string> success   = new List<string>();
    public List<string> clear_if  = new List<string>();
    public List<string> int_if    = new List<string>();
    public List<string> skill     = new List<string>();

    public BaseNpc() { }
    public BaseNpc(string name, Vector3 pos, float _textSpeed, int textidx, int type)
    {
        npcName = name;
        npcPostion = pos;
        this.textSpeed = _textSpeed;
        textIndex = textidx;
        npcImage = Resources.Load<Sprite>("NPC/Sprite" + name);
        npcType = (NpcType)type;
    }
    
    //해당하는 NPC의 대화를 불러와야함.
    public virtual void Load_Story(string path)
    {
        TextAsset xml = OpenXml(path);

        if (xml != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());
            XmlNode root = xmlDoc.DocumentElement;

            if(root.HasChildNodes)
            {
                XmlNodeList child = root.ChildNodes;

                for (int i=0; i<child.Count; i++)
                {
                    XmlNode temp = child[i];
                    XmlNodeList list = temp.ChildNodes;

                    sceen.Add(list[0].InnerXml);
                    restart.Add(list[1].InnerXml);
                    code.Add(list[2].InnerXml);
                    success.Add(list[3].InnerXml);
                    jump.Add(list[4].InnerXml);
                    point.Add(list[5].InnerXml);     
                    character.Add(list[6].InnerXml);
                    textName.Add(list[7].InnerXml);
                    int_if.Add(list[8].InnerXml);
                    clear_if.Add(list[9].InnerXml);
                    select.Add(list[10].InnerXml);
                    selectA.Add(list[11].InnerXml);
                    selectB.Add(list[12].InnerXml);
                    textStory.Add(list[13].InnerXml);
                    skill.Add(list[14].InnerXml);
                }
            }
        }
        else
        { 
            Debug.Log("Load fail");
        }
    }

    public virtual TextAsset OpenXml(string path)
    {
        TextAsset textXml = Resources.Load("XML/" + path, typeof(TextAsset)) as TextAsset;
        return textXml;
    }

    //NPC의 표정을 바꿔주는 함수.
    public virtual void Change_Face()
    { }
    //캐릭터와 npc와의 거리 계산하는 함수

    public float PlayerDis(Transform npc, Transform target)
    {
       float charDis = Vector3.Distance(npc.transform.position, target.transform.position);

        return charDis;
    }

    //캐릭터와 npc의 거리가 일정 거리일 경우 npc가 캐릭터를 바라보게금 rot 
    public virtual void NpcRotTarget(Transform npc, Transform target, float speed=2.0f)
    {
        Vector3 dir = (target.position - npc.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        npc.rotation = Quaternion.Slerp(npc.rotation, lookRotation, Time.deltaTime * speed);
    }

    //gameobjet buttonA,B를 List로 받아서 count 설정 하면 코드 줄일수 있음.
    public void Choice_peek(GameObject buttonA, GameObject buttonB, Text tell, Text name, Image npcImage)
    {
        //선택지 선택 하는 부분 셀렉트에 a,b 선택지가 있는데 그 선택지가 있을 경우.
        if (select[textIndex] != "null")
        {
            if (character[textIndex] != null)
            {
                npcImage.sprite = Resources.Load<Sprite>("NPC/Sprite/" + character[textIndex]) as Sprite;
            }
            name.text = textName[textIndex];
            tell.text = null;

            buttonA.SetActive(true);
            Text text = buttonA.transform.GetComponent<Text>();
            text.text = selectA[textIndex];

            buttonB.SetActive(true);
            text = buttonB.transform.GetComponent<Text>();
            text.text = selectB[textIndex];
            pick = true;
        }
        else
        {
            buttonA.SetActive(false);
            buttonB.SetActive(false);
            pick = false;
        }
    }
   
    //나중에 퀘스트를 진행할경우 특정 퀘스트 상태에서 조건검사를 거는것으로 다음문장을 실행하기 위해서는. current index를 활용하여.
    public virtual void StoryTelling(Image npcImage, Text name, Text tell, GameObject buttonA, GameObject buttonB)
    {
        Choice_peek(buttonA, buttonB, tell, name, npcImage);

        if (pick != true)
        {
            if (character[textIndex] != null)
            {
                npcImage.sprite = Resources.Load<Sprite>("NPC/Sprite/" + character[textIndex]) as Sprite;
            }
            name.text = textName[textIndex];
            tell.text = textStory[textIndex];

            //퀘스트 생성.
            if (code[textIndex] != "null")
            {
                currentqwest = code[textIndex];
                bool key = QwestManager.Instance.qwest.ContainsKey(currentqwest); //키 값이 null거나 없을 경우 bool값 반환.
                if (!key)
                {
                    QwestManager.Instance.qwest.Add(currentqwest, false);        //키값이 없으면 추가를 하고 있으면 그냥 pass
                    QwestManager.Instance.CreateQwest(currentqwest);             //코드 추가되면서 퀘스트도 같이 생성.
                }
            }

            if(restart[textIndex] != "null")
            {
                currentIndex = textIndex;
            }

            if(success[textIndex] != "null")
            {
                QwestManager.Instance.qwest[success[textIndex]] = true;
            }

            //스킬 습득.
            if(skill[textIndex] != "null")
            {
                ActiveSkill(skill[textIndex]);
            }

            //퀘스트 몬스터 잡아오는 뭐 그런거 있을 경우.
            if(int_if[textIndex] != "null")
            {
                if (qwestValue >= Int32.Parse(int_if[textIndex]))
                {
                    textIndex = FindCount(point, jump[textIndex]);
                    qwestValue = 0;
                }
                else
                    textIndex = currentIndex;
                return;
            }

            //특정 순간에 해당퀘스트가 클리어 되어있을 경우 jump  // end 보다 먼저 검사해야함.
            if(clear_if[textIndex] != "null")
            {
                string key = clear_if[textIndex];
                if(ClearQwestCK(key))
                {
                    textIndex = FindCount(point, jump[textIndex]);
                    return;
                }
            }

            //문장이 끝낫거나 퀘스트 씬을 강제적으로 끊을때.
            if (sceen[textIndex] == "cut")
            {
                sceenCut = true;
            }
            else if (point[textIndex] == "end")
            {
                qwestUIState = false;
                textIndex = currentIndex;
                return;

            }
            textIndex++;
        }
    }
    //  잘 생각해보니 이렇게 안하면 비록 1회성 코드가 되어버리지만 이렇게 하지 않으면 세부적인 조절을 할 수 없게 됨 .
    public void CheckQwestCode(string npc, string str)
    {
        switch (npc)
        {
            case "pylia":
                {
                    switch (str)
                    {
                        case "ms101":
                            QwestManager.Instance.qwest[str] = true;
                            break;

                        case "ms102":
                            if (ClearQwestCK("ms111"))
                            {
                                textIndex = FindCount(code, "ms112");
                                currentIndex = textIndex;
                            }
                            break;
                    }
                    break;
                }

            case "pidellio":
                {
                    switch(str)
                    {
                        case "ms103":
                            QwestManager.Instance.qwest[str] = true;
                            break;

                        case "ms104":
                            if (ClearQwestCK("ms112"))
                            {
                            }
                            else if (ClearQwestCK("ms103"))
                            {
                                prevqwest = str;
                                //공격 스킬 획득.  UI컨트롤에서 관리해주면 될듯.
                                Debug.Log("공격 스킬 습득");
                            }
                            break;
                    }
                    break;
                }

            case "dick":
                {
                    switch(str)
                    {
                        case "ms104":
                            QwestManager.Instance.qwest[str] = true;
                            break;

                        case "ms105":
                            if (currentValue >= qwestValue)
                            {
                                Debug.Log("수련용 증표 퀘스트 완료");
                                //limitedcode = "ms106";                //리미트 코드 해제.
                                textIndex = FindCount(point, "ms106");  // 인덱스 이동.
                                currentIndex = textIndex;              // 인덱스 저장.
                                Debug.Log("브렌디쉬 스킬 습득");
                            }
                            else
                                prevqwest = str;
                            break;

                        case "ms106":
                            prevqwest = str;
                            break;
                    }
                    break;
                }

            default:
                break;
        }
    }

    public bool ClearQwestCK(string code)
    {
        bool key = QwestManager.Instance.qwest.ContainsKey(code); 
        if (key)
        {
            bool value = QwestManager.Instance.qwest[code];
            return value;
        }
        return key;
    }

    //클릭된 것과 현재 셀렉트에 잇는 것과 같은 지. 비교 검사후 선택지 종료후
    //대화 내용을 다시 띄어 주어야하는데.. 띄어 주는 거랑 끄는걸 같이 못하려나.
    public void CompareString(string hitName, List<GameObject> obj)
    {
        if (select[textIndex] == hitName)
        {
            //맞을 경우 해당 카운트로.
            string find = jump[textIndex];
            textIndex = FindCount(point, find);
        }
        else
        {
            textIndex += 1;
            Debug.Log("not compare");
        }
    }

    public int FindCount(List<string> list, string str)
    {
        for(int i = 0; i<list.Count; i++)
        {
            if(list[i] == str)
            {
                return i;
            }
        }
        Debug.Log("not find");
        return 0;
    }

    protected void GetComponent()
    {
        if (GameObject.Find("Player"))
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }
        coll = transform.GetComponent<CapsuleCollider>();
        mesh = transform.FindChild("Material").GetComponent<SkinnedMeshRenderer>();
        //base.ani  = this.GetComponent<Animation>();
    }

    public void ActiveSkill(string skill)
    {
        switch(skill)
        {
            case "Attack":
                Debug.Log("기본공격 획득");
                break;

            case "Brandish":
                Debug.Log("브랜디시 스킬 획득");
                break;

            default:
                break;
        }
    }
}
