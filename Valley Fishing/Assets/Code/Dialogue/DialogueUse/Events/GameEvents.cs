using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//not really used
public class GameEvents : Singleton<GameEvents>
{
	private event Action loadGameScene;
	protected UseStringEventCondition useStringEventCondition = new UseStringEventCondition();
	protected UseStringEventModifier useStringEventModifier = new UseStringEventModifier();
	public void CallLoadScene() {
		SceneManager.LoadScene("Game");
		loadGameScene?.Invoke();
	}

	public virtual void DialogueModifierEvents(string stringEvent, StringEventModifierType stringEventModifierType, string value = "") {

	}

	public virtual bool DialogueConditionEvents(string stringEvent, StringEventConditionType stringEventConditionType, string value = "") {
		return false;
	}
}
