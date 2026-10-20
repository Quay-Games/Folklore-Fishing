using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStateData : BaseData
{
    public Container_Enum<LevelController.State> LevelStateValues = new Container_Enum<LevelController.State>();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        switch (LevelStateValues.Value)
        {
            case LevelController.State.ReelingFish:
                AudioManager.Instance.PlayReelSound(FMODManager.Instance.ReelSound);
                GameManager.Instance.LevelController.SetState(LevelController.State.ReelingFish);
                break;
            default:
                break;
        }
        yield break;
    }
}
