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
        TextAsset xml = (TextAsset)Resources.Load("XML/" + path, typeof(TextAsset));
        //TextAsset xml = OpenXml(path);

        if (xml != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.text);
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
            text.text = "▶ " + selectA[textIndex];

            buttonB.SetActive(true);
            text = buttonB.transform.GetComponent<Text>();
            text.text = "▶ " + selectB[textIndex];
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
            //퀘스트 몬스터 잡아오는 뭐 그런거 있을 경우. 지금은 테스트중이니까 숫자가 있는데 나중에는 퀘스트 수락하고 벨류값 초기화 해줄것 !! 0으로 초기화를 해줘야함 꼭!!
            if (int_if[textIndex] != "null")
            {
                if (currentValue >= Int32.Parse(int_if[textIndex]))
                {
                    textIndex = FindCount(point, jump[textIndex]);
                    currentValue = 0;
                    //return;
                }
                else
                    currentIndex = textIndex;
            }

            //특정 순간에 해당퀘스트가 클리어 되어있을 경우 jump  // end 보다 먼저 검사해야함.
            if (clear_if[textIndex] != "null")
            {
                string key = clear_if[textIndex];
                if (ClearQwestCK(key))
                {
                    textIndex = FindCount(point, jump[textIndex]);
                }
            }

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
                    MonsterManager.Instance.QwestCodeCheck(currentqwest, false);
                }
            }

            if(restart[textIndex] != "null")
            {
                currentIndex = textIndex;
            }

            if(success[textIndex] != "null")
            {
                QwestManager.Instance.qwest[success[textIndex]] = true;
                MonsterManager.Instance.QwestCodeCheck(success[textIndex], true);
            }
            else if(success[textIndex] == "ending")
            {
                Debug.Log("엔딩 시작");
            }

            //스킬 습득.
            if(skill[textIndex] != "null")
            {
                ActiveSkill(skill[textIndex]);
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
                VirtualJoystick.instance.SkillButtonActive(0);
                break;

            case "Brandish":
                VirtualJoystick.instance.SkillButtonActive(1);
                break;

            case "Hast":
                VirtualJoystick.instance.SkillButtonActive(2);
                break;

            case "Heal":
                VirtualJoystick.instance.SkillButtonActive(3);
                break;

            case "RaisingSword":
                VirtualJoystick.instance.SkillButtonActive(4);
                break;

            case "Status":
                PlayerCtrl.instance.maxHP += 100;
                PlayerCtrl.instance.maxMP += 100;
                break;

            default:
                break;
        }
    }
}
