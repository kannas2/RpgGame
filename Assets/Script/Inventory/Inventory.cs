using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<GameObject> Slots = new List<GameObject>();
    public List<Item> items = new List<Item>();

    public GameObject slots;
    private ItemDatabase database;
    public GameObject invenimage;
    public GameObject itemDesc;

    //슬롯 스타트 지점 x,y
    float x = -281.0f;
    float y = 80.0f;

    int slotAmount = 0;

    void Start()
    {
        instance = this;
        database = ItemDatabase.Instance;

        //Inven size 3 row 7 col
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
    }

    public void ItemDescOff()
    {
        if (itemDesc.activeSelf)
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
}  
