using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using UnityEngine.UI;

[System.Serializable]

public class BaseNpc : MonoBehaviour
{
    public enum NpcType
    {
        Shop,
    }

    protected string    npcName;
    protected Vector3   npcPostion;
    public    Sprite    npcImage;

    public float     textSpeed;
    public int       textIndex;
    public int       currentIndex = 0;
    protected string currentqwest;
    public string    prevqwest;
    public NpcType   npcType;

    public bool sceenCut      = false;
    public bool qwestUIState   = true;
    public string selectButton = null;
    public bool pick          = false;

    //퀘스트 나중에 몇마리 잡아오라 이런거 할때 사용될변수.
    protected string particularQwest;
    protected int qwestScore;

    //쉐이더.
    protected Shader shader1;
    protected Shader shader2;
    protected MeshRenderer mesh;

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
        npcImage = Resources.Load<Sprite>("NPC/" + name);
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
                //npc이미지 교체.
                npcImage.sprite = Resources.Load<Sprite>("NPC/" + character[textIndex]) as Sprite;
            }
            name.text = textName[textIndex];
            tell.text = null;

            buttonA.SetActive(true);
            Text text = buttonA.transform.FindChild("selectA").GetComponent<Text>();
            text.text = selectA[textIndex];

            buttonB.SetActive(true);
            text = buttonB.transform.FindChild("selectB").GetComponent<Text>();
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
    public virtual void StoryTelling(Image npcImage, Text name, Text tell, GameObject buttonA, GameObject buttonB, GameObject qwestButton)
    {
        Choice_peek(buttonA, buttonB, tell, name, npcImage);

        if (pick != true)
        {
            if (character[textIndex] != null)
            {
                npcImage.sprite = Resources.Load<Sprite>("NPC/" + character[textIndex]) as Sprite;
            }
            name.text = textName[textIndex];
            tell.text = textStory[textIndex];

            //퀘스트 코드가 바뀌거나 문장이 끝날경우.
            if (code[textIndex] != "null")
            {
                if (currentqwest == null)
                {
                    currentqwest = code[textIndex];
                    QwestManager.Instance.qwest.Add(currentqwest, false);
                }
                else
                {
                    if (prevqwest != currentqwest)
                    {
                        prevqwest = currentqwest;
                        QwestManager.Instance.qwest[prevqwest] = true; //이전 코드 완료.
                    }

                    currentqwest = code[textIndex];
                    currentIndex = FindCount(code, currentqwest);

                    bool key = QwestManager.Instance.qwest.ContainsKey(currentqwest); //키 값이 null거나 없을 경우 bool값 반환.
                    if (key != true)
                    {
                        QwestManager.Instance.qwest.Add(currentqwest, false); //키값이 없으면 추가를 하고 있으면 그냥 pass
                        QwestManager.Instance.CreateQwest(currentqwest); //코드 추가되면서 퀘스트도 같이 생성.
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
                textIndex = currentIndex; //여기서 값을 대입해주고 밑에가서 텍스트를 넣으니까 그전 텍스트가 입력되지..
                                          // 그전에는 scereencut이 true가 되버리면 바로 off했으니까.. 안생겻던 버그인데..
                return;
            }
          
            textIndex++;
            CheckQwestCode(currentqwest);
        }
    }
    public void CheckQwestCode(string code)
    {
        switch(code)
        {
            case "abc":
                if(qwestScore >= 40)
                {

                }
                textIndex = currentIndex;
                //퀘스트 진행창을 띄어준다던가.

                break;

            default:
                break;
        }
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
