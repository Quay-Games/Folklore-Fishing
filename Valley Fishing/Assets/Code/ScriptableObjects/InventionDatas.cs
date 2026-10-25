using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
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

	public void OnValidate() {
		List<string> inventionNames = new List<string>();
		for (int i = 0; i < Datas.Length; i++) {
			inventionNames.Add(Datas[i].ItemName.Replace(" ", ""));
		}

		StringBuilder sb = new StringBuilder();
		sb.AppendLine("public enum InventionName");
		sb.AppendLine("{");

		foreach (var name in inventionNames) {
			sb.AppendLine($"    {name},");
		}

		sb.AppendLine("}");
		File.WriteAllText("Assets/Code/InventionNames.cs", sb.ToString());

		AssetDatabase.Refresh();
	}
}
