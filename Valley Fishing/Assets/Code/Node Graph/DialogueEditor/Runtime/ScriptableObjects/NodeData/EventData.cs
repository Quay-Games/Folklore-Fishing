using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
//TODO GRIFF: Look into being able to pass integers and other things based on the event type.
public class EventData : BaseData {
	public EventType EventType;
	public EventData_EventName EventData_EventName;

	public EventData() {
		EventData_EventName = new EventData_EventName();
		EventType = EventType.None;
	}

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
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
                break;
            case EventType.FinishChallenge:
                break;
            case EventType.NpcEvent:
                break;
            case EventType.GiveMoney:
                break;
            default:
                break;
        }
        yield break;
    }
}
