using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.DialogueEditor;
using UnityEditor.Experimental.GraphView;
[System.Serializable]
public class BaseData
{
    public string NodeGuid;
    public Vector2 Position;
    //may not need to return a bool, we'll see
    public virtual bool Run(IDialogueRuntime runtime)
    {
        return false;
    }
}
