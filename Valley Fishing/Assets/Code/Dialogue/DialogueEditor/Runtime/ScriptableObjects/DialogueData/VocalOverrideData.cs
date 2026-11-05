using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class VocalOverrideData : BaseData
{
    public Container_EventReference VoiceEvent = new Container_EventReference();
    public Container_Enum<VoiceEventScenarios> VoiceEventScenario = new Container_Enum<VoiceEventScenarios>();
    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        //bool finished = false;

        //void OnFinished()
        //{
        //    finished = true;
        //}

        //SubscribeToCorrectEvent(OnFinished);

        //try
        //{
        //    if (VoiceEvent.Value.IsNull)
        //    {
        //        Debug.LogWarning("No voice event assigned to this dialogue node.");
        //        ProgressGraph.Value = true; //skip this node if no voice event is assigned
        //    }
        //    else
        //    {
        //        AudioManager.Instance.PlayVoiceOver(VoiceEvent.Value);
        //    }
        //    DialogueTextData tmp = DialogueText;

        //    //Only wait if we say to wait
        //    if (!ProgressGraph.Value)
        //    {

        //    }
        //}
        //finally
        //{
        //    AudioManager.Instance.OnVoiceLineOver -= OnFinished;
        //}
        yield return null;
    }

    private void SubscribeToCorrectEvent(Action callbackFunction)
    {
        //Subscribe to the correct event based on the selected scenario
        switch (VoiceEventScenario.Value)
        {
            case VoiceEventScenarios.FishCatch:
                GameManager.Instance.LevelController.OnFishCaught += callbackFunction;
                break;
        }
    }

    private void UnsubscribeFromCorrectEvent(Action callbackFunction)
    {
        //Unsubscribe from the correct event based on the selected scenario
        switch (VoiceEventScenario.Value)
        {
            case VoiceEventScenarios.FishCatch:
                GameManager.Instance.LevelController.OnFishCaught -= callbackFunction;
                break;
        }
    }
}