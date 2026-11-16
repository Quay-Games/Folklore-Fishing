using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneChangeData : BaseData
{
    public Container_Enum<SceneNames> SceneValues = new Container_Enum<SceneNames>("Scene");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        //load the scene via index
        SceneManager.LoadScene((int)SceneValues.Value);
        yield break;
    }
}