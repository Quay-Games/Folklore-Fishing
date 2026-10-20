using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GiveBaitData : BaseData
{
    public Container_Enum<BaitName> BaitValues = new Container_Enum<BaitName>();
    public Container_Int amount = new Container_Int();

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        InventoryManager.Instance.GiveBait(BaitValues.Value, amount.Value);
        yield break;
    }
}