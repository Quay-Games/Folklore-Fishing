using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputStateData : BaseData
{
    public Container_Enum<InputController.State> InputStateValues = new Container_Enum<InputController.State>("Input State");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        switch (InputStateValues.Value)
        {
            default:
                GameManager.Instance.InputController.SetState(InputStateValues.Value);
                break;
        }
        yield break;
    }
}
