using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<GameObject> Slots = new List<GameObject>();
    public List<Item> items = new List<Item>();

    public  GameObject slots;
    private Item_Database database;
    public GameObject invenimage;
    public GameObject itemDesc;

    //슬롯 스타트 지점 x,y 나중에 세부 조절이 필요함 망할 UI좌표계 
    float x = -281.0f;
    float y = 80.0f;

    int slotAmount = 0;

    void Start()
    {
        instance = this;
        database = GameObject.Find("ItemDatabase").GetComponent<Item_Database>();
        
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 7; k++)
            {
                //슬롯 생성 셋팅.
                GameObject slot = (GameObject)Instantiate(slots);
                slot.GetComponent<Slot>().slotNumber = slotAmount;

                Slots.Add(slot);
                items.Add(new Item());

                slot.transform.SetParent(invenimage.transform);

                slot.name = "Slot" + i + "(" + k + ")";

                //pos set
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + 92.0f;

                if (k == 6)
                {
                    x = -281.0f;
                    y = y - 107f;
                }
                slotAmount++;
            }
        }
        invenimage.SetActive(false);
        //아이템 생성 할때 
        //addItem(1);
    }
    public void ItemDescOff()
    {
        if(itemDesc.activeSelf)
        {
            itemDesc.SetActive(false);
        }
    }

    public void ItemDescContent(int slotNum)
    {
        itemDesc.SetActive(true);
        itemDesc.transform.FindChild("icon").GetComponent<Image>().sprite = items[slotNum].itemIcon;
        itemDesc.transform.FindChild("desc").GetComponent<Image>().sprite = items[slotNum].itemDesc;
    }

    //나중에 아이템 코드를 string 형식으로 바꾸던가 .. 지금처럼 그냥 숫자를 쓰던가.
    public void addItem(int id)
    {
        for (int i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemID == id)
            {
                Item item = database.items[i];
                addItemAtEmptySlot(item, id);
                break;
            }
            else
                Debug.Log("not find Item");
        }
    }

    //나중에 반복문 말고 값으로 찾는거 알아볼것.
    public bool ChkId(int id)
    {
        for (int idx = 0; idx < items.Count; idx++)
        {
            if (items[idx].itemID == id)
            {
                items[idx].itemValue += 1;
                return false;
            }
        }
        return true;
    }

    void addItemAtEmptySlot(Item item, int id)
    {
        if (ChkId(id))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == null)
                {
                    items[i] = item;
                    break;
                }
            }
        }
    }

    //아이템을 추가할때 기존에 아이템이 있는지 판단여부와 기존에 아이템이 잇을 경우 add value 없을 경우 item add 판정.
    /*
        public int slotsX, slotsY;
        public GUISkin sloat_skin;

        public List<Item> inventory = new List<Item>();
        public List<Item> slots = new List<Item>();

        private bool showInventory;
        private Item_Database database;
        private bool showTooltip;
        private string tooltip;

        // Use this for initialization
        void Start ()
        {
            slotsX = 5;
            slotsY = 4;

            for(int idx = 0; idx<slotsX * slotsY; idx++)
            {
                slots.Add(new Item());
                inventory.Add(new Item());
            }

            if(GameObject.Find("ItemDatabase"))
            {
                database = GameObject.Find("ItemDatabase").GetComponent<Item_Database>();
            }

            //AddItem(0);
            //Debug.Log("inventory : " + InventoryContains(2));
            //RemoveItem(0);
        }

        void Update () {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                showInventory = !showInventory;
            }
        }

        void OnGUI()
        {
            tooltip = "";
            GUI.skin = sloat_skin;
            if(showInventory)
            {
                DrawInventory();
            }
            if(showTooltip)
            {
                GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y,200,200), tooltip);
            }
        }

        void DrawInventory()
        {
            int idx = 0;
            for(int x = 0; x<slotsX; x++)
            {
                for(int y= 0; y<slotsY; y++)
                {
                    Rect slotRect = new Rect(x * 60, y * 60, 50, 50);
                    GUI.Box(new Rect(x * 60, y * 60, 50, 50),"",sloat_skin.GetStyle("Sloat"));

                    slots[idx] = inventory[idx];

                    if (slots[idx].itemName != null)
                    {
                        //GUI.DrawTexture(slotRect, slots[idx].itemIcon);
                        if(slotRect.Contains(Event.current.mousePosition))
                        {
                            tooltip = CreateTooltip(inventory[idx]);
                            showTooltip = true;
                        }
                    }

                    if(tooltip == "")
                    {
                        showTooltip = false;
                    }
                    idx++;
                }
            }
        }

        string CreateTooltip(Item item)
        {
            tooltip = item.itemName;
            return tooltip;
        }

        void RemoveItem(int id)
        {
            for(int i=0; i<inventory.Count; i++)
            {
                if(inventory[i].itemID == id)
                {
                    inventory[i] = new Item();
                    break;
                }
            }
        }
        void AddItem(int id)
        {
            for(int idx= 0; idx<inventory.Count; idx++)
            {
                if(inventory[idx].itemName == null)
                {
                    for(int j=0; j<database.items.Count; j++)
                    {
                        if(database.items[j].itemID == id)
                        {
                            inventory[idx] = database.items[j];
                        }
                    }
                    break;
                }
            }
        }

        bool InventoryContains(int id)
        {
            bool result = false;
            for(int i=0; i<inventory.Count; i++)
            {
                result = inventory[i].itemID == id;
                if(result)
                {
                    break;
                }
            }
            return result;
        }
        */
}
