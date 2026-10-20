using System;
using System.Collections.Generic;
using UnityEngine;

public class InventionManager : Singleton<InventionManager> {

	[SerializeField] private InventionDatas inventionDatas;


	public List<InventionDatas.InventionData> CurrrentIventions;
	public List<OwnedItemTypeData> CraftableInventions {
		get {
			List<OwnedItemTypeData> inventions = new List<OwnedItemTypeData>();
			for (int i = 0; i < inventionDatas.Datas.Length; i++) {
				for (int j = 0; j < inventionDatas.Datas[i].InventionComponents.Length; j++) {
					if (InventoryManager.Instance.OwnedLootNames.Contains(inventionDatas.Datas[i].InventionComponents[j].Item)) {
						OwnedItemTypeData inventionTypeCatchData = new OwnedItemTypeData();
						inventionTypeCatchData.quantity = 0;
						inventionTypeCatchData.OwnedItemData = inventionDatas.Datas[i];
						if (!inventions.Contains(inventionTypeCatchData)) {
							inventions.Add(inventionTypeCatchData);
						}
					}
				}
			}
			return inventions;
		}
	}

	public void BuyInvention(InventionDatas.InventionData invention) {
		for (int i = 0; i < invention.InventionComponents.Length; i++) {
			for (int j = 0; j < InventoryManager.Instance.OwnedLootTypeDatas.Count; j++) {
				string inventionName = InventoryManager.Instance.OwnedLootTypeDatas[j].OwnedItemData.ItemName.Replace(" ", "");
				if (invention.InventionComponents[i].Item.ToString() == inventionName) {
					InventoryManager.Instance.OwnedLootTypeDatas[j].quantity -= invention.InventionComponents[i].quantity;
				}
			}
		}
		this.CurrrentIventions.Add(invention);
	}
	public void InventionDestroyed(InventionDatas.InventionData invention) {
		this.CurrrentIventions.Remove(invention);
	}
}
