using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class UIOverrideData : BaseData
{
    public Container_ScriptableObject<UIControlEvent> overrideEvent = new Container_ScriptableObject<UIControlEvent>();
    public Container_Enum<UIType> buttonToOverride = new Container_Enum<UIType>();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        bool hasTriggered = false;

        bool Trigger()
        {
            hasTriggered = true;
            return overrideEvent.Value.ActionInstead();
        }

        UIManager.Instance.DoInstead = Trigger;
        UIManager.Instance.IfType = buttonToOverride.Value;

        while(!hasTriggered)
        {
            yield return null;
        }

        UIManager.Instance.DoInstead = null;
        UIManager.Instance.IfType = UIType.DEFAULT;
    }
}
