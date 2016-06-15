using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Item      item;
    public Image     itemImage;
    public int       slotNumber;
    public Inventory  inventory;
    public Text      itemValue;

    void Start()
    {
        inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemValue = gameObject.transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inventory.items[slotNumber].itemName != null)
        {
            item = inventory.items[slotNumber];

            //나중에 두개로 합칠것임 지금은 일단 대충.
            itemImage.enabled = true;
            itemValue.enabled = true;

            itemImage.sprite = inventory.items[slotNumber].itemIcon;
            itemValue.text = inventory.items[slotNumber].itemValue.ToString();
        }
        else
        {
            itemImage.enabled = false;
            itemValue.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (itemImage.sprite != null)
        {
            inventory.ItemDescContent(slotNumber);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
    }
}