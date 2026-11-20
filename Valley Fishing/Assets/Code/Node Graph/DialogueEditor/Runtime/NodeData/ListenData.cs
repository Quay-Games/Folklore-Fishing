using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class ListenData : BaseData
{
	public Container_ScriptableObject<ListenEvent> listenEventContainer = new Container_ScriptableObject<ListenEvent>();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        yield return listenEventContainer.Value.StartListening();
    }
}
