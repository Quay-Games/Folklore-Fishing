using FMODUnity;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitDatas", menuName = "Scriptable Objects/BaitDatas")]
public class BaitDatas : ScriptableObject {
	[System.Serializable]
	public class FishSpawnChance {
		public FishName FishName;
		public float SpawnChance;
	}

	[System.Serializable]
	public class BaitData : BaseItemData {
		public bool IsTutorial;
		public List<FishSpawnChance> FishSpawnChances = new List<FishSpawnChance>();
	}

	public BaitData[] Datas;
    public void OnValidate()
    {
        List<string> baitNames = new List<string>();
        for (int i = 0; i < Datas.Length; i++)
        {
            baitNames.Add(Datas[i].ItemName.Replace(" ", ""));
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum BaitName");
        sb.AppendLine("{");

        foreach (var name in baitNames)
        {
            sb.AppendLine($"    {name},");
        }

        sb.AppendLine("}");
        File.WriteAllText("Assets/Code/ScriptableObjects/Enums/BaitNames.cs", sb.ToString());

        AssetDatabase.Refresh();
    }
}
