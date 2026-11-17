using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//A fairly hardcoded system for playing music, I fully intend to automate this somehow, instead of having to update the switch constantly
public class PlayMusicData : BaseData
{
    public Container_Enum<MusicTrack> MusicTrackHolder = new Container_Enum<MusicTrack>("Track");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        switch(MusicTrackHolder.Value)
        {
            case MusicTrack.LEVEL:
                AudioManager.Instance.PlayMusic(FMODManager.Instance.NormalLevelMusic);
                break;
            case MusicTrack.BOSS:
                AudioManager.Instance.PlayMusic(FMODManager.Instance.BossLevelMusic);
                break;
        }
        yield break;
    }
}
