using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Start Match Event")]
[System.Serializable]
public class StartMatch : DialogueEvent
{
	public override void RunEvent() {
		GameEvents.Instance.CallLoadScene();
	}
}
