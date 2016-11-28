using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Entities;
using RAIN.Entities.Aspects;

[RAINAction("Get Stand On Point")]
public class GetStandOnPoint : RAINAction
{
    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// AspectVariable is the variable that contains the aspect we detected, and whose Entity should have a related "standhere" aspect
    /// *Don't use quotes when typing in the variable name
    /// </summary>    
    public Expression AspectVariable = new Expression();

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// StandOnPointVariable is the variable we'll be assigning our stand on point aspect to.  The aspect can be used as a move target.
    /// *Don't use quotes when typing in the variable name
    /// </summary>    
    public Expression StandOnPointVariable = new Expression();

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (!AspectVariable.IsVariable)
            throw new Exception("The Get Stand On Point node requires a valid AspectVariable.");
        if (!StandOnPointVariable.IsVariable)
            throw new Exception("The Get Stand On Point node requires a valid StandOnPointVariable.");

        RAINAspect tAspect = ai.WorkingMemory.GetItem<RAINAspect>(AspectVariable.VariableName);
        if (tAspect == null)
            return ActionResult.FAILURE;

        Entity tEntity = tAspect.Entity;
        RAINAspect tStandOnAspect = tEntity.GetAspect("standhere");
        if (tStandOnAspect == null)
            return ActionResult.FAILURE;

        ai.WorkingMemory.SetItem<RAINAspect>(StandOnPointVariable.VariableName, tStandOnAspect);

        return ActionResult.SUCCESS;
    }
}