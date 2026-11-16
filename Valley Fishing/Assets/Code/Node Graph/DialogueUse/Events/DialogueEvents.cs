using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//unusued as far as I can see
public class DialogueEvents : GameEvents
{
	public override bool DialogueConditionEvents(string stringEvent, StringEventConditionType stringEventConditionType, string value = "") {
		string eventName = stringEvent;
		if(eventName.Contains("Npc")){
			eventName = eventName.Replace("Npc","");
		}
		switch (stringEvent.ToLower()) {
			case "money":
				int money = 0;
				return useStringEventCondition.ConditionFloatCheck((money), int.Parse(value),stringEventConditionType);
			default:
				//Debug.LogWarning("No stringEvent was fount");
				return false;
		}
	}

	public override void DialogueModifierEvents(string stringEvent, StringEventModifierType stringEventModifierType, string value = "" ) {
		switch (stringEvent.ToLower()) {
			case "money":
				break;
			case "team":
				break;
			case "objective":
				break;
			default:
				Debug.LogWarning("No stringEvent was fount");
				break;
		}
	}
}
