using UnityEngine;
using System.Collections;

public class NpcDilliseu : BaseNpc {

    public static NpcDilliseu instance; //나중에 몬스터 마리수 올리는거 때문에.

    void Start()
    {
        instance = this;
        Init();

        base.npcPostion = new Vector3(256.0f, 65.0f, .0f);
        base.npcName        = "dilliseu";
        base.textSpeed      = 1.0f;
        base.textIndex      = 0;
        base.npcType        = 0;
        base.qwestValue     = 20;         //잡아올 몬스터 값.
        base.currentValue   = 0;
        base.prevCheckQwest = "ms106";    // 이전 퀘스트를 완료해야지만 클릭이가능.

        //쉐이더
        base.shader1 = Shader.Find("Outlined/Diffuse");
        base.shader2 = Shader.Find("Standard");

        Load_Story(npcName);
    }

    private void Init()
    {
        base.GetComponent();
    }

    void FixedUpdate()
    {
        if (base.PlayerDis(transform, base.target.transform) < 5.0f)
        {
            base.NpcRotTarget(transform, target.transform, 3.0f);
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
