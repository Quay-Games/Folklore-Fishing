using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class VocalOverrideData : BaseData
{
    public Container_EventReference VoiceEvent = new Container_EventReference();
    public Container_Enum<VoiceEventScenarios> VoiceEventScenario = new Container_Enum<VoiceEventScenarios>("Voice Line Scenario");
    public Container_Bool ProgressGraph = new Container_Bool("Progress when line stars");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        if(VoiceEvent.Value.IsNull)
        {
            Debug.LogWarning("No voice event assigned to this vocal override node.");
            yield break;
        }
        bool lineTriggered = false;
        bool lineFinished = false;

        void OnLineTriggered()
        {
            lineTriggered = true;
        }

        void OnLineFinished(bool skipped)
        {
            lineFinished = true;
        }

        SubscribeToCorrectEvent(OnLineTriggered);
        try
        {
            while (!lineTriggered)
            {
                yield return null;
            }

            UnsubscribeFromCorrectEvent(OnLineTriggered);

            AudioManager.Instance.OnVoiceLineOver += OnLineFinished;
            AudioManager.Instance.PlayVoiceOver(VoiceEvent.Value);
            while(!lineFinished && !ProgressGraph.Value)
            {
                yield return null;
            }

            AudioManager.Instance.OnVoiceLineOver -= OnLineFinished;
        }
        finally
        {
            //just in case it never got there originally
            UnsubscribeFromCorrectEvent(OnLineTriggered);
        }
        yield return null;
    }

    private void SubscribeToCorrectEvent(Action callbackFunction)
    {
        //Subscribe to the correct event based on the selected scenario
        switch (VoiceEventScenario.Value)
        {
            case VoiceEventScenarios.FishCatch:
                GameManager.Instance.LevelController.FishView.FishCatchLineToPlay = callbackFunction;
                break;
        }
    }

    private void UnsubscribeFromCorrectEvent(Action callbackFunction)
    {
        //Unsubscribe from the correct event based on the selected scenario
        switch (VoiceEventScenario.Value)
        {
            case VoiceEventScenarios.FishCatch:
                if(GameManager.Instance.LevelController.FishView.FishCatchLineToPlay == callbackFunction)
                {
                    GameManager.Instance.LevelController.FishView.FishCatchLineToPlay = null;
                }
                break;
        }
    }
}