using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitDatas", menuName = "Scriptable Objects/BaitDatas")]
public class BaitDatas : ScriptableObject {

	[System.Serializable]
	public class PerLocationFishSpawnChance {
		public FishName FishName;
		public float SpawnChance;
	}


	[System.Serializable]
	public class FishSpawnChance {
		public PerLocationFishSpawnChance[] PerLocationFishSpawnChance;
	}

	[System.Serializable]
	public class BaitData : BaseItemData {
		public FishSpawnChance[] FishSpawnChances;
	}

	public BaitData[] Datas;
}
