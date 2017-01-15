using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class rotateToEnemy : RAINAction
{
    private GameObject humanSeen;

    public override void Start(RAIN.Core.AI ai) {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai) {
        // Get the target
        Vector3 humanPos = ai.WorkingMemory.GetItem<Vector3>("humanPosition");
        humanSeen = ai.WorkingMemory.GetItem<GameObject>("humanSeen");

        if (humanSeen != null)
        {
            Debug.Log(humanPos);
        }
        else
        {
            Debug.Log("No human seen");
        }
            //ai.Body.transform.LookAt(humanSeen.transform.position);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}