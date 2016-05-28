using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System;
using System.IO;

public class Item_Database : MonoBehaviour {

    List<Dictionary<string, string>> mContainer = new List<Dictionary<string, string>>();

    string fileName = "ItemList";

    public List<Item> items = new List<Item>();

	void Awake()
    {
        TextAsset textXml = Resources.Load("XML/"+ fileName, typeof(TextAsset)) as TextAsset; //어차피 한번 읽고 버려질 지역변수.

        if(textXml != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textXml.ToString());
            XmlNode root = xmlDoc.DocumentElement;

            if(root.HasChildNodes)
            {
                XmlNodeList child = root.ChildNodes;
                for(int i=0; i<child.Count; i++)
                {
                    XmlNode temp = child[i];
                    XmlNodeList itemList = temp.ChildNodes;

                    items.Add(new Item(itemList[0].InnerXml,
                                 Convert.ToInt32(itemList[1].InnerXml),
                                 Convert.ToInt32(itemList[2].InnerXml),
                                 itemList[3].InnerXml,
                                 Convert.ToInt32(itemList[4].InnerXml)));
                }
            }
            
        }
        
        //items.Add(new Item("mp_portion",2, 1, "mp_portion", 1)); //원래는 숫자가아닌 Item.ItemType.Consumable 형식이였는데 XML로 변경하면서 바뀌게되었음.

    }

    //예를 들면
    //뭐.. 기존에 아이템이 존재 하지 않으면 ADD 만약 존재한다면 그 존재하는 숫자 cnt 에 가산을 하여 숫자만 늘리는것.
}
