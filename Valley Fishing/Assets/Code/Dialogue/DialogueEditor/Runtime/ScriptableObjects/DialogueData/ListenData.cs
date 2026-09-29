using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public List<Container_ListenEventSO> Container_ListenEventSOs = new List<Container_ListenEventSO>();

    public override bool Run(IDialogueRuntime runtime)
    {
        Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered += ListenEventTriggered;
        Container_ListenEventSOs[0].ListenEventSO.RunEvent(runtime.Runner);
        return true;
    }

    private void ListenEventTriggered()
    {
        Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered -= ListenEventTriggered;
        //will need access to the runner here, let me think on it
        //StopAllCoroutines();
        //Continue(0);
    }
}
