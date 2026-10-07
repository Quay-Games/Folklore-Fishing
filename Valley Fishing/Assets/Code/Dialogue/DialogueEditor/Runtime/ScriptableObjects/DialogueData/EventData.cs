using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EventData : BaseData {
	public EventType EventType;
	public EventData_EventName EventData_EventName;

	public EventData() {
		EventData_EventName = new EventData_EventName();
		EventType = EventType.None;
	}

    public override void Run(IDialogueRuntime runtime)
    {
        switch (EventType)
        {
            case EventType.None:
                break;
            case EventType.StartObjective:
                break;
            case EventType.FinishObjective:
                break;
            case EventType.StartChallenge:
                return;
            case EventType.FinishChallenge:
                break;
            case EventType.NpcEvent:
                break;
            case EventType.GiveMoney:
                break;
            default:
                break;
        }
    }
}
