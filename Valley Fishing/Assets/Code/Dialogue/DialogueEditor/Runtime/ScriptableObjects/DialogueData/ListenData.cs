using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public List<Container_ListenEventSO> Container_ListenEventSOs = new List<Container_ListenEventSO>();

    public override void Run(IGraphRunnerRuntime runtime)
    {
        Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered += ListenEventTriggered(runtime);
        Container_ListenEventSOs[0].ListenEventSO.RunEvent(runtime.GetMonoBehaviour());
    }

    private System.Action ListenEventTriggered(IGraphRunnerRuntime runtime)
    {
        //I hope this works the way I think it does
        return () =>
        {
            Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered -= ListenEventTriggered(runtime);
            runtime.StopCoroutines();
        };
    }
}
