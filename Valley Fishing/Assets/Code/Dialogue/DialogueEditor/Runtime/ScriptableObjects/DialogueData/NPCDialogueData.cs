using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class NPCDialogueData : BaseData {
    public Container_EventReference VoiceEvent = new Container_EventReference();
    public Container_Bool ProgressGraph = new Container_Bool("Progress Immediately");
	public DialogueTextData DialogueText;
    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        bool finished = false;

        void OnFinished(bool skipped)
        {
            finished = true;
        }

        AudioManager.Instance.OnVoiceLineOver += OnFinished;

        try
        {
            AudioManager.Instance.PlayVoiceOver(VoiceEvent.Value);
            DialogueTextData tmp = DialogueText;

            //Only wait if we say to wait
            if(!ProgressGraph.Value)
            {
                while (!finished)
                {
                    yield return null;
                }
            }
        }
        finally
        {
            AudioManager.Instance.OnVoiceLineOver -= OnFinished;
        }
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
