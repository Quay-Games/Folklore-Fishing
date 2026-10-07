using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public List<Container_ListenEventSO> Container_ListenEventSOs = new List<Container_ListenEventSO>();

    public override void Run(IDialogueRuntime runtime)
    {
        Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered += ListenEventTriggered(runtime);
        Container_ListenEventSOs[0].ListenEventSO.RunEvent(runtime.GetMonoBehaviour());
    }

    private System.Action ListenEventTriggered(IDialogueRuntime runtime)
    {
        //I hope this works the way I think it does
        return () =>
        {
            Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered -= ListenEventTriggered(runtime);
            runtime.StopCoroutines();
        };
    }
}
