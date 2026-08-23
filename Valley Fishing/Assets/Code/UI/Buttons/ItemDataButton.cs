using FMODUnity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDataButton : ButtonVoiceOverComponent
{
    #region Serialized Fields

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemSellPrice;
	[SerializeField] private TMP_Text itemQuantity;

    #endregion


    #region Properties

    public OwnedItemTypeData ItemData { get; set; }

    #endregion


    #region Public Methods

    public void AssignData(OwnedItemTypeData data)
    {
        this.ItemData = data;
        if (itemName != null)
        {
            itemName.text = data.OwnedItemData.ItemName;
        }
        if (itemImage != null)
        {
            itemImage.sprite = data.OwnedItemData.ItemImage;
        }
        if (itemSellPrice != null)
        {
            itemSellPrice.text = data.OwnedItemData.ItemSellPrice.ToString() + "g";
        }
		if(itemQuantity != null) {
			itemQuantity.text = "x" + data.quantity .ToString();
		}
    }

    public override void DoHoverEffect()
    {
        base.DoHoverEffect();
        List<EventReference> voiceoverChain = new List<EventReference>();
        
        if (this.ItemData.OwnedItemData is FishDatas.FishData)
        {
            int itemIndex = Array.IndexOf(InventoryManager.Instance.FishDatas.Datas, this.ItemData.OwnedItemData);
            int intemQuantity = InventoryManager.Instance.OwnedFishTypeDatas[itemIndex].quantity;
            voiceoverChain.Add(this.ItemData.OwnedItemData.ItemNameEvent);
            voiceoverChain.AddRange(FMODManager.Instance.GetNumber(this.ItemData.OwnedItemData.ItemSellPrice));
            voiceoverChain.Add(FMODManager.Instance.Gold);
            voiceoverChain.Add(FMODManager.Instance.YouHave);
            for (int i = 0; i < FMODManager.Instance.GetNumber(intemQuantity).Count; i++)
            {
                voiceoverChain.Add(FMODManager.Instance.GetNumber(intemQuantity)[i]);
            }            
        }

        if(this.ItemData.OwnedItemData is BaitDatas.BaitData)
        {
			if (GameManager.Instance.ShopController != null) {
				int itemIndex = Array.IndexOf(InventoryManager.Instance.BaitDatas.Datas, this.ItemData.OwnedItemData);
				int baitQuantity = GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[itemIndex];
				int itemValue = InventoryManager.Instance.OwnedBaitTypeDatas[itemIndex].OwnedItemData.ItemSellPrice * baitQuantity;
				voiceoverChain.Add(this.ItemData.OwnedItemData.ItemNameEvent);
				voiceoverChain.AddRange(FMODManager.Instance.GetNumber(itemValue));
				voiceoverChain.Add(FMODManager.Instance.Gold);
				if (GameManager.Instance.ShopController.BaitShop.BaitQuantities[itemIndex] == 0) {
					voiceoverChain.Add(FMODManager.Instance.SoldOut);
					AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
					return;
				}
				voiceoverChain.AddRange(FMODManager.Instance.GetNumber(GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[itemIndex]));
				voiceoverChain.Add(FMODManager.Instance.Left);
			}
			else {
				int itemIndex = Array.IndexOf(InventoryManager.Instance.BaitDatas.Datas, this.ItemData.OwnedItemData);
				GameManager.Instance.LevelController.BaitView.BaitSelected(itemIndex);
				int intemQuantity = InventoryManager.Instance.OwnedBaitTypeDatas[itemIndex].quantity;
				for (int i = 0; i < FMODManager.Instance.GetNumber(intemQuantity).Count; i++) {
					voiceoverChain.Add(FMODManager.Instance.GetNumber(intemQuantity)[i]);
				}
				voiceoverChain.Add(this.ItemData.OwnedItemData.ItemNameEvent);
			}
		}
        AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
    }

	public override void OnSubmit(BaseEventData eventData) {
		if (this.ItemData.OwnedItemData is FishDatas.FishData)
        {
            GameManager.Instance.ShopController.BaitShop.SellFish(transform.GetSiblingIndex());
        }
		if (this.ItemData.OwnedItemData is BaitDatas.BaitData) {
			if (GameManager.Instance.ShopController != null) {
				int baitQuantity = GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[transform.GetSiblingIndex()];
				GameManager.Instance.ShopController.BaitShop.BuyBait(transform.GetSiblingIndex(), baitQuantity);
			}
			else {
				GameManager.Instance.LevelController.BaitView.BaitClicked(Array.IndexOf(InventoryManager.Instance.BaitDatas.Datas, this.ItemData.OwnedItemData));
			}
		}
    }

    #endregion

}
