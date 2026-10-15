using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public List<Container_ListenEvent> listenEvents = new List<Container_ListenEvent>();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        listenEvents[0].ListenEvent.OnEventTriggered += ListenEventTriggered(runtime);
        listenEvents[0].ListenEvent.RunEvent(runtime.GetMonoBehaviour());
        //this will probably need to be changed to something that waits for the event to trigger, but for now this will stay
        yield break;
    }

    private System.Action ListenEventTriggered(IGraphRunnerRuntime runtime)
    {
        //I hope this works the way I think it does
        return () =>
        {
            listenEvents[0].ListenEvent.OnEventTriggered -= ListenEventTriggered(runtime);
            runtime.StopCoroutines();
        };
    }
}
