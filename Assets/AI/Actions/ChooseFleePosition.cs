using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;

[RAINAction("Choose Flee Position")]
public class ChooseFleePosition : RAINAction
{
    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// FleeDistance is the max range to use when picking a flee target
    /// </summary>
    public Expression FleeDistance = new Expression();

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// FleeFrom indicates the position or game object of the entity we are fleeing from
    /// </summary>
    public Expression FleeFrom = new Expression();

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// StayOnGraph is a boolean (true/false) that indicates whether the flee target must be on the nav graph
    /// </summary>
    public Expression StayOnGraph = new Expression();

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// FleeTargetVariable is the name of the variable that the result will be assigned to
    /// *Don't use quotes when typing in the variable name
    /// </summary>
    public Expression FleeTargetVariable = new Expression();

    /// <summary>
    /// The default flee distance to use when the FleeDistance is invalid
    /// </summary>
    private float _defaultFleeDistance = 10f;

    /// <summary>
    /// A reusable MoveLookTarget for storing the position of the FleeFrom object
    /// </summary>
    private MoveLookTarget _fleeTarget = new MoveLookTarget();

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (!FleeTargetVariable.IsVariable)
            throw new Exception("The Choose Flee Position node requires a valid Flee Target Variable");

        float tFleeDistance = 0f;
        if (FleeDistance.IsValid)
            tFleeDistance = FleeDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

        if (tFleeDistance <= 0f)
            tFleeDistance = _defaultFleeDistance;

        //Start by trying to flee away from the FleeFrom
        if (FleeFrom.IsVariable)
            MoveLookTarget.GetTargetFromVariable(ai.WorkingMemory, FleeFrom.VariableName, ai.Motor.DefaultCloseEnoughDistance, _fleeTarget);
        else
            _fleeTarget.TargetType = MoveLookTarget.MoveLookTargetType.None;
        
        if (_fleeTarget.IsValid)
        {
            //Start by just running the opposite direction
            Vector3 tAway = ai.Kinematic.Position - _fleeTarget.Position;
            Vector3 tFleeDirection = tAway.normalized * UnityEngine.Random.Range(1f, tFleeDistance);

            Vector3 tFleePosition = ai.Kinematic.Position + tFleeDirection;
            if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
            {
                ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
                return ActionResult.SUCCESS;
            }

            //Check forty five away
            Vector3 tFortyFive = Quaternion.Euler(new Vector3(0, 45, 0)) * tFleeDirection;
            tFleePosition = ai.Kinematic.Position + tFortyFive;
            if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
            {
                ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
                return ActionResult.SUCCESS;
            }

            //Check minus forty five away
            tFortyFive = Quaternion.Euler(new Vector3(0, -45, 0)) * tFleeDirection;
            tFleePosition = ai.Kinematic.Position + tFortyFive;
            if (ai.Navigator.OnGraph(tFleePosition, ai.Motor.MaxHeightOffset))
            {
                ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tFleePosition);
                return ActionResult.SUCCESS;
            }
        }

        //We could not find a reasonable flee target, so just choose a random point
        Vector3 tDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
        tDirection *= tFleeDistance;

        Vector3 tDestination = ai.Kinematic.Position + tDirection;
        if (StayOnGraph.IsValid && (StayOnGraph.Evaluate<bool>(ai.DeltaTime, ai.WorkingMemory)))
        {
            if (!ai.Navigator.OnGraph(tDestination, ai.Motor.MaxHeightOffset))
                return ActionResult.FAILURE;
        }

        ai.WorkingMemory.SetItem<Vector3>(FleeTargetVariable.VariableName, tDestination);

        return ActionResult.SUCCESS;
    }
}