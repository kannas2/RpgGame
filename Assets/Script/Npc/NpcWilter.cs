using UnityEngine;
using System.Collections;

public class NpcWilter : BaseNpc {

    public static NpcWilter instance; //나중에 몬스터 마리수 올리는거 때문에.

    void Start()
    {
        instance = this;
        Init();

        base.npcPostion = new Vector3(256.0f, 65.0f, .0f); //나중에 지울예정 혹시몰라서 일단 대기중인변수.
        base.npcName = "wilter";
        base.textSpeed = 1.0f;
        base.textIndex = 0;
        base.npcType = 0;
        base.qwestValue = 1;       //잡아올 몬스터 값.
        base.currentValue = 1;       //지금은 테스트중이니 이미 잡은걸로.
        base.prevCheckQwest = "ms110";

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
