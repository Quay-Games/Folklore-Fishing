using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartData: BaseData {
    public override void Run(IDialogueRuntime runtime)
    {
        //this node will always run the next node, and doesnt do anything else
    }
}
