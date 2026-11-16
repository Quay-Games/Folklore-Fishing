using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class CatchOverrideData : BaseData
{
    public Container_Enum<FishName> FishToCatch = new Container_Enum<FishName>("Fish");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        bool catchTriggered = false;

        FishName OnCatch()
        {
            catchTriggered = true;
            return FishToCatch.Value;
        }

        try
        {
            GameManager.Instance.LevelController.FishToSpawn = OnCatch;

            while (!catchTriggered)
            {
                yield return null;
            }
        }
        finally
        {
            GameManager.Instance.LevelController.FishToSpawn = null;
        }   
        yield return null;
    }
}