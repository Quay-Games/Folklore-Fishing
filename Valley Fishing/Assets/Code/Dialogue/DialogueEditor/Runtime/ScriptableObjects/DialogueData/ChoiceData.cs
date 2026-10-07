using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ChoiceData : BaseData {

#if UNITY_EDITOR
	public TextField TextField{
		get;
		set;
	}
#endif

	public Container_ChoiceStateType ChoiceStateTypes = new Container_ChoiceStateType();
	public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();
	public List<EventData_StringCondition> EventData_StringConditions = new List<EventData_StringCondition>();

    public override void Run(IDialogueRuntime runtime)
    {
		//the original choicedata implementation didnt actually have any running behavior, so this will sit empty for now
    }
}
