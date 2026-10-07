using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BranchData : BranchingTypeData
{
	public List<EventData_StringCondition> EventData_StringConditions = new List<EventData_StringCondition>();

    public override void Run(IGraphRunnerRuntime runtime)
    {
        bool checkBranch = true;
        foreach (EventData_StringCondition item in EventData_StringConditions)
        {
            if (!GameEvents.Instance.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.StringEventValue.Value))
            {
                checkBranch = false;
                break;
            }
        }

        choice = (checkBranch ? 0 : 1);
    }
}
