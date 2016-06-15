using UnityEngine;
using System.Collections;

public class MonsterManager : Singleton<MonsterManager>
{
    public GameObject weakA;
    public GameObject weakB;
    public GameObject weakC;
    public GameObject strong;
    public GameObject Boss;

    public string[] qwestList;

    public void Start()
    {
        qwestList = new string[]
        {
          "ms105","ms107","ms109","ms112","ms115"
        };
    }

    public void QwestCodeCheck(string code, bool state)
    {
        for (int i=0; i<qwestList.Length; i++)
        {
            if(qwestList[i] == code)
            {
                MonsterStage(qwestList[i], state);
            }
        }
    }

    public void MonsterStage(string code , bool state)
    {
        switch(code)
        {
            case "ms105":
                weakA.SetActive(!state);
                break;

            case "ms107":
                weakB.SetActive(!state);
                break;

            case "ms109":
                weakC.SetActive(!state);
                break;

            case "ms112":
                strong.SetActive(!state);
                break;

            case "ms115":
                Boss.SetActive(!state);
                break;

            default:
                break;
        }
    }
}
