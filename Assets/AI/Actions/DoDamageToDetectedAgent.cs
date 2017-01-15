using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.BehaviorTrees;

[RAINAction]
public class DoDamageToDetectedAgent : RAINAction
{
	private GameObject targetAgent = null;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		// Get the target
		targetAgent = ai.WorkingMemory.GetItem<GameObject>("targetNear");


		if (targetAgent != null) {
			if (targetAgent.GetComponent<AIAgent> ().isDead ()) {
				return ActionResult.SUCCESS;
			} else {
				targetAgent.GetComponent<AIAgent> ().getDamage ();
				return ActionResult.FAILURE;
			}
		}
			
			
        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}