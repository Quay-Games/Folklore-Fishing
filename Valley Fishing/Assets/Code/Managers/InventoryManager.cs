using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static InventoryManager;

public class InventoryManager : Singleton<InventoryManager> {


	public FishDatas FishDatas;
	public List<OwnedItemTypeData> OwnedFishTypeDatas;
	public List<OwnedItemTypeData> OwnedLootTypeDatas;
    public int TotalOwnedFish
    {
        get
        {
            int count = 0;
            for (int i = 0; i < this.OwnedFishTypeDatas.Count; i++)
            {
                count += this.OwnedFishTypeDatas[i].quantity;
            }
            return count;
        }
    }

    public BaitDatas BaitDatas;
	public List<OwnedItemTypeData> OwnedBaitTypeDatas;

	public int TotalOwnedBaits { get
		{
			int count = 0;
			for (int i = 0; i < this.OwnedBaitTypeDatas.Count; i++)
			{
				count += this.OwnedBaitTypeDatas[i].quantity;
			}
			return count;
		} 
	}

	public BaitDatas.BaitData CurrentBait;

	public bool OneBaitTypeLeft {
		get {
			int typesOfBait = 0;
			for (int i = 0; i < this.OwnedBaitTypeDatas.Count; i++) {
				if(this.OwnedBaitTypeDatas[i].quantity > 0) {
					typesOfBait++;
				}
				if(typesOfBait == 2) {
					return false;
				}
			}
			return true;
		}
	}

	public int GetNumberOfSpecificFish(string fishName)
	{
        foreach (var fish in OwnedFishTypeDatas)
        {
            if(fish.OwnedItemData.ItemName.Equals(fishName))
			{
				return fish.quantity;
			}
        }
		return -1;
    }
    public override void Awake() {
		base.Awake();
		for (int i = 0; i < this.FishDatas.Datas.Length; i++) {
			if (this.FishDatas.Datas[i].IsLoot) {
				OwnedItemTypeData lootTypeCatchData = new OwnedItemTypeData();
				lootTypeCatchData.quantity = 0;
				lootTypeCatchData.OwnedItemData = this.FishDatas.Datas[i];
				OwnedLootTypeDatas.Add(lootTypeCatchData);
				continue;
			}
            OwnedItemTypeData fishTypeCatchData = new OwnedItemTypeData();
			fishTypeCatchData.quantity = 0;
			fishTypeCatchData.OwnedItemData = this.FishDatas.Datas[i];
            OwnedFishTypeDatas.Add(fishTypeCatchData);
		}
		for (int i = 0; i < this.BaitDatas.Datas.Length; i++)
		{
            OwnedItemTypeData baitTypeData = new OwnedItemTypeData();
            baitTypeData.quantity = 0;
            baitTypeData.OwnedItemData = this.BaitDatas.Datas[i];
			OwnedBaitTypeDatas.Add(baitTypeData);

        }
	}

}
