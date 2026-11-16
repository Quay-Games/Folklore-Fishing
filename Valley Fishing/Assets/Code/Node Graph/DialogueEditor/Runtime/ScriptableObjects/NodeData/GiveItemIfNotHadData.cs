using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[System.Serializable]
public class GiveItemIfNotHadData : BaseData
{
    public Container_EnumSelect<ItemCategories> ChosenItem = new Container_EnumSelect<ItemCategories>(new List<Type> { typeof(FishName), typeof(BaitName) }, newLabel: "Item Category");
    public Container_Int amount = new Container_Int("Amount");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        switch (ChosenItem.listCategory)
        {
            case ItemCategories.Fish:
                FishName fishName = (FishName)ChosenItem.Value;
                InventoryManager.Instance.SetFishIfNotHad(fishName, amount.Value);
                break;
            case ItemCategories.Bait:
                BaitName baitName = (BaitName)ChosenItem.Value;
                InventoryManager.Instance.SetBaitIfNotHad(baitName, amount.Value);
                break;
        }
        yield break;
    }
}