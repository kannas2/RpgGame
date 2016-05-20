using UnityEngine;
using System.Collections;

public class NpcFidelio : BaseNpc
{
    private string fileName;
    public Transform target;
    private CapsuleCollider coll;

    void Start()
    {
        fileName = "fidelio";

        base.npcPostion = new Vector3(256.0f, 65.0f, .0f);
        base.npcName    = "fidelio";
        base.textSpeed  = 1.0f;
        base.textIndex  = 0;
        base.npcType    = 0;
        base.particularQwest = "aniqwest";
        base.qwestScore = 0;

        //쉐이더
        base.mesh = transform.FindChild("Material").GetComponent<MeshRenderer>();
        base.shader1 = Shader.Find("Outlined/Diffuse");
        base.shader2 = Shader.Find("Standard");

        Load_Story(fileName);
        coll = transform.GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if(base.PlayerDis(transform, target.transform) < 5.0f)
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
