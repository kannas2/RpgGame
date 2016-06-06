using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcPylia : BaseNpc {

    public static NpcPylia instance;

	void Start ()
    {
        instance = this;
        Init();

        base.npcPostion     = new Vector3(256.0f, 65.0f, .0f); //나중에 지울예정 혹시몰라서 일단 대기중인변수.
        base.npcName        = "pylia";
        base.textSpeed      = 1.0f; //이것도 시간적 여유가 있다면.. 스토리가 한글자씩 나오게 설정할때 쓰려고 선언해두었던..
        base.textIndex      = 0;
        base.npcType        = 0;
        base.qwestValue      = 0;
        base.currentqwest    = null;
        base.prevCheckQwest  = "ms101";

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
