using UnityEngine;
using System.Collections;

public class NpcFidelio : BaseNpc
{
    void Start()
    {
        GetComponent();
        
        base.npcPostion = new Vector3(256.0f, 65.0f, .0f);
        base.npcName    = "pidellio";
        base.textSpeed  = 1.0f;
        base.textIndex  = 0;
        base.npcType    = 0;
        base.qwestValue = 0;
        base.limitedcode = "ms104";

        //쉐이더
        base.shader1 = Shader.Find("Outlined/Diffuse");
        base.shader2 = Shader.Find("Standard");

        //이전NPC의 퀘스트를 완료 했는지 판단 그전NPC의 퀘스트를 완료 하지 않았을 경우 클릭이 되지 않게 설정하기 위해서.
        base.prevCheckQwest = "ms101";

        Load_Story(npcName);
    }

    private void GetComponent()
    {
        if (GameObject.Find("Player"))
        {
            base.target = GameObject.Find("Player").GetComponent<Transform>();
        }
        base.coll = transform.GetComponent<CapsuleCollider>();
        base.mesh = transform.FindChild("Material").GetComponent<MeshRenderer>();
        //base.ani  = this.GetComponent<Animation>();
    }

    void FixedUpdate()
    {
        if(base.PlayerDis(transform, base.target.transform) < 5.0f)
        {
            base.NpcRotTarget(transform,target.transform, 3.0f);
            coll.enabled = true;
            mesh.material.shader = shader1;
        }
        else
        {
            coll.enabled = false;
            mesh.material.shader = shader2;
        }
    }
}
