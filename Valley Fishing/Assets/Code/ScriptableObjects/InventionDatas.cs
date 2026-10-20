using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InventionDatas", menuName = "Scriptable Objects/InventionDatas")]
public class InventionDatas : ScriptableObject {

	[System.Serializable]
	public class InventionData: BaseItemData {
		public InventionComponent[] InventionComponents;
	}

	[System.Serializable]
	public class InventionComponent {
		public FishName Item;
		public int quantity;
	}

	public InventionData[] Datas;
}
