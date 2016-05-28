﻿using UnityEngine;
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
    protected int qwestValue;

    //그전 NPC의 퀘스트를 완료 했는지 여부 확인할 변수.
    public string prevCheckQwest;

    //캐릭터 애니메이션
    protected Animator ani;
    
    //쉐이더.
    protected Shader shader1;
    protected Shader shader2;
    protected MeshRenderer mesh; //나중에 애니가 들어간 캐릭터 올때 SkinnedMeshRenderer로 교체할것.

    // npc대화 창 설정할  list
    public List<string> textStory  = new List<string>();
    public List<string> textName   = new List<string>();
    public List<string> code      = new List<string>();
    public List<string> point     = new List<string>();
    public List<string> character  = new List<string>();
    public List<string> select    = new List<string>();
    public List<string> jump      = new List<string>();
    public List<string> color     = new List<string>();
    public List<string> selectA   = new List<string>();
    public List<string> selectB   = new List<string>();
    public List<string> sceen     = new List<string>();

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
                    code.Add(list[1].InnerXml);
                    point.Add(list[2].InnerXml);
                    character.Add(list[3].InnerXml);
                    textName.Add(list[4].InnerXml);
                    select.Add(list[5].InnerXml);
                    jump.Add(list[6].InnerXml);
                    color.Add(list[7].InnerXml);
                    selectA.Add(list[8].InnerXml);
                    selectB.Add(list[9].InnerXml);
                    textStory.Add(list[10].InnerXml);
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

            //퀘스트 코드가 바뀌거나 문장이 끝날경우.
            if (code[textIndex] != "null")
            {
                if (currentqwest == null)
                {
                    currentqwest = code[textIndex];
                    bool key = QwestManager.Instance.qwest.ContainsKey(currentqwest);
                    if (!key)
                    {
                        QwestManager.Instance.qwest.Add(currentqwest, false);
                    }
                }
                else
                {   //limitedcode를 추가한 이유는 초기에 기초를 잘못 잡았기 때문에.. 일명 땜빵코드.. 일단 이 limitedcode는 각각 npc가 밑에서 check code에서 npc가
                    //말을 할 수 있는 대사의 최대치를 조절해줌. 그리고 이 코드를 쓴 이유는 npc가 새로운 퀘스트 코드를 부여받을때 같은 npc에게 말을 걸면 퀘스트가 완료되
                    //버리기 때문에 리밋코드를 추가함. 리밋코드는 계속 변경될 예정.
                    if (prevqwest != currentqwest)
                    {
                        if(currentqwest != limitedcode)
                        { 
                            prevqwest = currentqwest;
                            QwestManager.Instance.qwest[prevqwest] = true; //이전 코드 완료.
                        }
                    }
                    Debug.Log("리미트코드 : " + limitedcode);
                    currentqwest = code[textIndex];
                    currentIndex = FindCount(code, currentqwest);

                    try
                    {
                        bool key = QwestManager.Instance.qwest.ContainsKey(currentqwest); //키 값이 null거나 없을 경우 bool값 반환.
                        if (!key)
                        {
                            QwestManager.Instance.qwest.Add(currentqwest, false); //키값이 없으면 추가를 하고 있으면 그냥 pass
                            QwestManager.Instance.CreateQwest(currentqwest);     //코드 추가되면서 퀘스트도 같이 생성.
                        }
                        //예외처리. try catch로 예외처리 할까 했는데 이게 좀더 효율적이라고 판단함.
                        //오류 코드 Argument Exception : An element with the same key.
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                    }
                }
            }

            if (sceen[textIndex] == "cut")
            {
                sceenCut = true;
            }
            else if (point[textIndex] == "end")
            {
                qwestUIState = false;
                textIndex = currentIndex;    //여기서 값을 대입해주고 밑에가서 텍스트를 넣으니까 그전 텍스트가 입력되지..
                CheckQwestCode(currentqwest); //문장의 끝을 만나면 코드 확인하고 예외처리 확인후 다음 문단으로 넘어갈지 확인.
                return;
            }
            textIndex++;
        }
    }
    //  잘 생각해보니 이렇게 안하면 비록 1회성 코드가 되어버리지만 이렇게 하지 않으면 세부적인 조절을 할 수 없게 됨 .
    public void CheckQwestCode(string str)
    {
        switch (str)
        {
            case "ms104":
                if (ClearQwestCK("ms103"))
                {
                    //공격 스킬 획득.  UI컨트롤에서 관리해주면 될듯.
                    Debug.Log("공격 스킬 습득");
                }
                
                break;

            //필리아에게 다시 찾아갈 때 실행될 코드.
            case "ms102":
                if (ClearQwestCK("ms111"))
                {
                    textIndex = FindCount(code, "ms112");
                    currentIndex = textIndex;
                }
                break;

            case "ms113":
                if(ClearQwestCK(str))
                {
                    textIndex = FindCount(code, "ms113");
                    currentIndex = textIndex;
                }
                break;

            default:
                break;
        }
    }

    public bool ClearQwestCK(string code)
    {
        bool key = QwestManager.Instance.qwest.ContainsKey(code);
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
}
