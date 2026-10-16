using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public Container_ListenEvent listenEventContainer;

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        yield return listenEventContainer.ListenEvent.StartListening();
    }
}
