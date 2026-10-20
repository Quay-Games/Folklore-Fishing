using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EndData: BaseData {
	public Container_Enum<EndNodeType> EndNodeType = new Container_Enum<EndNodeType>();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        //end nodes dont do anything in and of themselves at the moment
        yield break;    
    }
}
