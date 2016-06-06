using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIControl : Singleton<UIControl>
{
    public Text charName;
    public Text story;
    public GameObject buttonA;
    public GameObject buttonB;
    public Text  notifly;
    public Image charImage;

    public GameObject virtualJoystick;
    public GameObject screen_UI;
    public GameObject qwest_UI;
    public GameObject InventoryImage;
    public GameObject qwestImage;
    public Camera UICamera;

    private BaseNpc npc;

    public bool show_UI;
    private bool showIventory;
    private bool showQwest;

    public List<string> screenText = new List<string>();
    public List<GameObject> button = new List<GameObject>();
    private float ftime;

    void Start()
    {
        show_UI = true;
        button.Add(buttonA);
        button.Add(buttonB);
       
        //게임 공지로 사용될 list 밑에 함수에서 update 에서 지속적으로 내용을 확인하여 내용이 있을 경우 게임 화면에 글을 띄어줌.
        screenText.Add("파비앙! 너에게 꼭 해야할 말이 있어!");
        screenText.Add("나는 발렌시아 마을 중심가에 있어.");
        screenText.Add("나를 찾아와줘!");
    }

    void FixedUpdate()
    {
        //2d raycast
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 pos = UICamera.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray2d = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit2d = Physics2D.Raycast(ray2d.origin, ray2d.direction);

            if (hit2d.collider != null)
            {
                if (hit2d.transform.CompareTag("SelectButton"))
                {
                    npc.CompareString(hit2d.transform.name, button);
                    npc.pick = false;
                }
                else if (hit2d.transform.CompareTag("QwestTitle"))
                {
                    QwestManager.Instance.CompleteQwest(hit2d.transform.name);
                    Debug.Log("hit : " + hit2d.transform.name);
                }
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

        //NPC 클릭 하는 것만 3D.
        if (Input.GetKeyDown(KeyCode.Mouse0) && show_UI == true)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "NPC")
                    {
                        npc = hit.transform.GetComponent<BaseNpc>();
                        Debug.Log("npc bool :" + npc.prevCheckQwest);
                        
                        //비어있거나 true일경우에만 npc클릭이 가능하게끔 예외처리. 
                        bool check = QwestManager.Instance.qwest.ContainsKey(npc.prevCheckQwest);
                        if (check)
                        {
                            npc.currentIndex = npc.textIndex;
                            SetSceen(npc);
                        }
                    }
                }
            }
        }

        //NPC대화창이 켜져있을 경우. 퀘스트 선택중 선택창이 떳을 경우 이벤트 예외처리 할것. 클릭에 딜레이좀 줄것 .. 너무 빨라서 더블클릭이 일어남.
        if (show_UI != true && npc != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (npc.textStory.Count > npc.textIndex && npc.pick != true) // index over 예외처리.
                {
                    if (npc.sceenCut != true)
                    {
                        npc.StoryTelling(charImage, charName, story, buttonA, buttonB);
                    }
                    else
                    {
                        Screen_switch();
                        npc.sceenCut = false;
                    }
                }
                //여기 위치가 아닌 raycast 위에 저기다 ray.transform.name를 받아와야하니 여기가 아닌 저 위에 설정할것.
                //선택지 선택 하는 함수 생성. 현재 select를 받아와서 그 셀렉트와 선택한 것과 같다면 / 틀리다면을 설정 후 pick false로 변경.
                //다음 클릭서 부터는 변경된 이야기로 진행이 됨.
            }
            //qwestUI State.
            if (npc.qwestUIState != true)
            {
                Screen_switch();
                npc.qwestUIState = true;
            }
        }
        //게임 스크린 공지 텍스트.
        if (screenText.Count > 0)
        {
            GameNotify();
        }
    }

    //UIControl에서 사용하는 모든 변수들을 값을 초기화 시켜줌. 이 함수는 거절 버튼을 눌렀을 경우 호출 하게 할 예정임.
    public void InitValue()
    {
        npc.textIndex = npc.currentIndex;
    }

    //NPC에게 얻어온 대화 List를 UIControl에서 복사하여 사용 매번 다른 NPC와 대화할때마다 모든 값을 초기화 시켜줘야함.
    public void SetSceen(BaseNpc npc)
    {
        Screen_switch();
        //그냥 이미지만 세팅해주자.
        charImage.sprite = npc.npcImage;
    }

    //NPC대화 화면 ON/OFF 스위칭
    public void Screen_switch()
    {
        show_UI = !show_UI;
        virtualJoystick.SetActive(show_UI);
        screen_UI.SetActive(show_UI);
        qwest_UI.SetActive(!show_UI);
    }

    //게임 내에 공지 할 수 있는 기능
    public void GameNotify()
    {
        ftime += Time.deltaTime;
        if (ftime <= 2.0f)
        {
            notifly.text = screenText[0];
        }
        else
        {
            ftime = .0f;
            notifly.text = null;
            screenText.RemoveAt(0);
        }
    }

    public void Inventory_Button()
    {
        showIventory = !showIventory;
        InventoryImage.SetActive(showIventory);
    }

    public void Qwest_Button()
    {
        showQwest = !showQwest;
        qwestImage.SetActive(showQwest);
        QwestManager.Instance.QwestListInit();
    }
}
