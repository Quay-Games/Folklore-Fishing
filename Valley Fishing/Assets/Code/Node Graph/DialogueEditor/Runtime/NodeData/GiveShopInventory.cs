using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[System.Serializable]
public class GiveShopInventoryData : BaseData
{
    public Container_EnumSelect<ShopType> ChosenItem = new Container_EnumSelect<ShopType>(new List<Type> { typeof(FishName), typeof(BaitName) }, newLabel: "Item Category");
    public Container_Int amount = new Container_Int("Amount");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        //switch (ChosenItem.listCategory)
        //{
        //    case ItemCategories.Fish:
        //        FishName fishName = (FishName)ChosenItem.Value;
        //        InventoryManager.Instance.SetFishIfNotHad(fishName, amount.Value);
        //        break;
        //    case ItemCategories.Bait:
        //        BaitName baitName = (BaitName)ChosenItem.Value;
        //        InventoryManager.Instance.SetBaitIfNotHad(baitName, amount.Value);
        //        break;
        //}
        yield break;
    }
}