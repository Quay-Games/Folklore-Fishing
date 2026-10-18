using FMODUnity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventionDatas;

public class ItemDataButton : ButtonVoiceOverComponent
{
    #region Serialized Fields

    public TMP_Text itemName;
	public Image itemImage;
	public TMP_Text itemSellPrice;
	public TMP_Text itemQuantity;
	public Transform additionalButtonInfoTransform;
	public ItemDataButton itemDataButton;

	#endregion


	#region Properties
	public Transform AdditionalButtonInfoTransform => additionalButtonInfoTransform;
	public OwnedItemTypeData ItemData { get; set; }

    #endregion


    #region Public Methods

    public void AssignData(OwnedItemTypeData data)
    {
        this.ItemData = data;
        if (itemName != null) {
            itemName.text = data.OwnedItemData.ItemName;
        }
        if (itemImage != null) {
            itemImage.sprite = data.OwnedItemData.ItemImage;
        }
        if (itemSellPrice != null) {
            itemSellPrice.text = data.OwnedItemData.ItemSellPrice.ToString() + "g";
        }
		if(itemQuantity != null) {
			itemQuantity.text = "x" + data.quantity .ToString();
		}
		if (additionalButtonInfoTransform != null) {
			List<ItemDataButton> buttons = new List<ItemDataButton>();
			List<InventionDatas.InventionComponent> inventionComponents = new List<InventionDatas.InventionComponent> ();
			InventionDatas.InventionData inventionData = data.OwnedItemData as InventionDatas.InventionData;
			for (int i = 0; i < inventionData.InventionComponents.Length ; i++) {
				inventionComponents.Add((data.OwnedItemData as InventionDatas.InventionData).InventionComponents[i]);
			}
			for (int i = 0; i < inventionComponents.Count; i++) {
				buttons.Add(Instantiate(itemDataButton,additionalButtonInfoTransform));
				buttons[i].itemName.text = inventionComponents[i].Item.ToString();
				buttons[i].itemQuantity.text = inventionComponents[i].quantity.ToString();
				buttons[i].itemImage.sprite = InventoryManager.Instance.GetLoot(inventionComponents[i].Item).ItemImage;
			}
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
            for (int i = 0; i < FMODManager.Instance.GetNumber(intemQuantity).Count; i++) {
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
				if (GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[itemIndex] == 0) {
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
		if (this.ItemData.OwnedItemData is InventionDatas.InventionData) {
			InventionDatas.InventionData invention = this.ItemData.OwnedItemData as InventionDatas.InventionData;
			voiceoverChain.Add(this.ItemData.OwnedItemData.ItemNameEvent);
			voiceoverChain.Add(FMODManager.Instance.MadeByCombining);
			for (int i = 0; i < invention.InventionComponents.Length; i++) {
				for (int j = 0; j < FMODManager.Instance.GetNumber(invention.InventionComponents[i].quantity).Count; j++) {
					voiceoverChain.Add(FMODManager.Instance.GetNumber(invention.InventionComponents[i].quantity)[j]);
					voiceoverChain.Add(InventoryManager.Instance.GetLoot(invention.InventionComponents[i].Item).ItemNameEvent);
					if (i != invention.InventionComponents.Length - 1) {
						voiceoverChain.Add(FMODManager.Instance.And);
					}
				}
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
				int baitIndex = System.Array.IndexOf(InventoryManager.Instance.BaitDatas.Datas, this.ItemData.OwnedItemData);
				int baitQuantity = GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[baitIndex];
				GameManager.Instance.ShopController.BaitShop.BuyBait(baitIndex, baitQuantity);
			}
			else {
				GameManager.Instance.LevelController.BaitView.BaitClicked(Array.IndexOf(InventoryManager.Instance.BaitDatas.Datas, this.ItemData.OwnedItemData));
			}
		}
		if (this.ItemData.OwnedItemData is InventionDatas.InventionData) {
			for (int i = 0; i < (this.ItemData.OwnedItemData as InventionDatas.InventionData).InventionComponents.Length; i++) {
				InventionDatas.InventionComponent component = (this.ItemData.OwnedItemData as InventionDatas.InventionData).InventionComponents[i];	
				if (component.quantity > InventoryManager.Instance.GetNumberOfSpecificLoot(component.Item.ToString().Replace(" ",""))) {
					return;
				}
			}
			InventionManager.Instance.BuyInvention(this.ItemData.OwnedItemData as InventionDatas.InventionData);
		}
	}

    #endregion

}
