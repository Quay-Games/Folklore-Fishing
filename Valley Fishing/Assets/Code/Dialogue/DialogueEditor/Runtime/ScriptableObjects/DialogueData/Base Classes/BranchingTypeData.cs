//I want a parent class here which can hold the two guids that go out of condition nodes, response nodes, and branch nodes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.DialogueEditor;
using UnityEditor.Experimental.GraphView;
[System.Serializable]
//A parent class for those nodes that have two branches, since they share behavior, such behavior is defined here
public abstract class BranchingTypeData : BaseData
{
    protected string branchOneNodeGuid;
    protected string branchTwoNodeGuid;
    protected int choice = 0;

    public string GetChosenNodeGuid()
    {
        if (choice == 0)
        {
            return branchOneNodeGuid;
        }
        else
        {
            return branchTwoNodeGuid;
        }
    }
    public void SetBranchOneNodeGuid(string guid)
    {
        branchOneNodeGuid = guid;
    }

    public void SetBranchTwoNodeGuid(string guid)
    {
        branchTwoNodeGuid = guid;
    }
}
