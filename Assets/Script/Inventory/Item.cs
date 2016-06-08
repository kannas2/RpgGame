using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest,
    }

    public string itemName;
    public int itemID;
    public int itemValue;

    public Sprite itemIcon;
    public Sprite itemDesc;

    public ItemType itemType;

    //아이템 이름, id, 개수, 약어, 타입
    public Item() { }
    public Item(string name, int id, int value, string desc, int type)
    {
        itemName = name;
        itemID = id;
        itemValue = value;
        itemDesc = Resources.Load<Sprite>("ItemSpriteImage/ItemContent/" + name);
        itemIcon = Resources.Load<Sprite>("ItemSpriteImage/" + name);
        itemType = (ItemType)type;
    }
}
