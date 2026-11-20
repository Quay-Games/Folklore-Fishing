using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EnterShopData : BaseData
{
    public Container_Enum<ShopType> selectedShop = new Container_Enum<ShopType>("Shop");

    public override IEnumerator Run(IGraphRunnerRuntime runtime)
    {
        switch (selectedShop.Value)
        {
            case ShopType.Bait:
                GameManager.Instance.ShopController.Shore.EnterBaitShop();
                break;
        }
        yield break;
    }
}
