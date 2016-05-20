using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcPylia : BaseNpc {

    [SerializeField]
    private Transform target;

    private string  fileName;
    private CapsuleCollider coll;


	void Start ()
    {
        fileName = "pylia";

        base.npcPostion = new Vector3(256.0f, 65.0f, .0f);
        base.npcName = "pylia";
        base.textSpeed = 1.0f;
        base.textIndex = 0;
        base.npcType = 0;
        base.particularQwest = "aniqwest";
        base.qwestScore = 0;
        base.currentqwest = null;

        Load_Story(fileName);
        coll = transform.GetComponent<CapsuleCollider>();

        Load_Story(fileName);
    }

	void Update ()
    {
        if (base.PlayerDis(transform, target.transform) < 5.0f)
        {
            base.NpcRotTarget(transform, target.transform, 3.0f);
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }
    }
}
