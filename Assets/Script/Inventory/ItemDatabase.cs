using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;

public class ItemDatabase : Singleton<ItemDatabase> {

    string fileName = "ItemList";

    public List<Item> items = new List<Item>();

	void Awake()
    {
        TextAsset textXml = Resources.Load("XML/"+ fileName, typeof(TextAsset)) as TextAsset;

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
    }
}
