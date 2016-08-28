﻿using UnityEngine;
using System.Collections;

public class NpcFidelio : BaseNpc
{
    public static NpcFidelio instance;

    void Start()
    {
        instance = this;
        Init();
        
        base.npcPostion = new Vector3(256.0f, 65.0f, .0f);
        base.npcName        = "pidellio";
        base.textSpeed      = 1.0f;
        base.textIndex      = 0;
        base.npcType        = 0;
        base.qwestValue     = 1;
        base.currentValue   = 0;
        base.prevCheckQwest = "ms101";

        //쉐이더
        base.shader1 = Shader.Find("Outlined/Diffuse");
        base.shader2 = Shader.Find("Standard");

        //NPC 대사 불러오기
        Load_Story(npcName);
    }

    private void Init()
    {
        base.GetComponent();
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
