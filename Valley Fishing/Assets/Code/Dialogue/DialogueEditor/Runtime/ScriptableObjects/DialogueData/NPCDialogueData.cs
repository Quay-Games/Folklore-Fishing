using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class NPCDialogueData : BaseData {
	public EventReference VoiceEvent;
	public DialogueTextData DialogueText;

    public override void Run(IGraphRunnerRuntime runtime)
    {
        AudioManager.Instance.PlayVoiceOver(VoiceEvent);
        DialogueTextData tmp = DialogueText;
    }
}

[System.Serializable]
public class NPCDialogueData_BaseContainer {
	public Container_Int ID = new Container_Int();
}

[System.Serializable]
public class DialogueTextData : NPCDialogueData_BaseContainer {
	public Container_String GuidID = new Container_String();
	public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();
}
