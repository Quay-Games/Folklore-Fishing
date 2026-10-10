using System;
using System.Collections.Generic;
using UnityEngine;

public class InventionManager : Singleton<InventionManager> {

	[SerializeField] private InventionDatas inventionDatas;


	public List<InventionDatas.InventionData> CurrrentIventions { get; set; }
	public List<OwnedItemTypeData> CraftableInventions {
		get {
			List<OwnedItemTypeData> inventions = new List<OwnedItemTypeData>();
			for (int i = 0; i < inventionDatas.Datas.Length; i++) {
				for (int j = 0; j < inventionDatas.Datas[i].InventionComponents.Length; j++) {
					if (InventoryManager.Instance.OwnedLootNames.Contains(inventionDatas.Datas[i].InventionComponents[j].Item)) {
						Debug.Log(inventionDatas.Datas[i]);
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

	public void InventionAdded(InventionDatas.InventionData invention) {
		this.CurrrentIventions.Add(invention);
	}
	public void InventionDestroyed(InventionDatas.InventionData invention) {
		this.CurrrentIventions.Remove(invention);
	}
}
