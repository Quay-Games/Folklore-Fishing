using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RodShopData", menuName = "Scriptable Objects/RodShopData")]
public class UpgradesData : ScriptableObject {

	[System.Serializable]
	public class UpgradeData {
		public List<int> UpgradePrices;
		public List<int> UpgradeValues;
		public List<EventReference> UpgradeHoverEvents;
		public List<EventReference> UpgradeBoughtEvents;
	}

	public UpgradeData ReelSpeedData;
	public UpgradeData StrafeSpeedData;
	public UpgradeData FailSpeedData;
	public UpgradeData HatData;
}
