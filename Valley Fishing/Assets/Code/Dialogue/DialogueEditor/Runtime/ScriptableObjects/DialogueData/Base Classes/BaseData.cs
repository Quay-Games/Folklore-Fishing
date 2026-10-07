using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.DialogueEditor;
using UnityEditor.Experimental.GraphView;
[System.Serializable]
public abstract class BaseData
{
    public string NodeGuid;
    public Vector2 Position;
    public abstract void Run(IGraphRunnerRuntime runtime);
}
