using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EndData: BaseData {
	public Container_EndNodeType EndNodeType = new Container_EndNodeType();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        //end nodes dont do anything in and of themselves at the moment
        yield break;    
    }
}
