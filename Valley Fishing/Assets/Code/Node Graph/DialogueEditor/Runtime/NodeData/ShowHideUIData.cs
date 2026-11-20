using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class ShowHideUIData : BaseData
{
    public Container_Enum<UIType> uiType = new Container_Enum<UIType>();
    public Container_Bool showContainer = new Container_Bool("Show/Hide");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        UIManager.Instance.ShowHideUIElement(uiType.Value, showContainer.Value);
        yield return null;
    }
}
