using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToggleCaptainHelpData : BaseData
{
    public Container_Bool ToggleCaptainHelp = new Container_Bool("On/Off");
    public Container_Int Seconds = new Container_Int("Seconds till Prompt");
    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        HelpManager.Instance.toggleReelHelp(ToggleCaptainHelp.Value, Seconds.Value);
        yield break;
    }
}
