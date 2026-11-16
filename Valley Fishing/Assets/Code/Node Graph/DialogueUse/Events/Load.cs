using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dialogue/Load Scene Event")]
[System.Serializable]
public class TestEvent : DialogueEvent
{
	public override void RunEvent() {
		GameEvents.Instance.CallLoadScene();
	}
}
