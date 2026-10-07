using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionData : BranchingTypeData
{
	public EventData_EventName EventData_EventName = new EventData_EventName();

    public override void Run(IDialogueRuntime runtime)
    {
        //this is useless at the moment, but we'll fill it out in future
        bool checkBranch = true;
        choice = (checkBranch ? 0 : 1);
    }
}
