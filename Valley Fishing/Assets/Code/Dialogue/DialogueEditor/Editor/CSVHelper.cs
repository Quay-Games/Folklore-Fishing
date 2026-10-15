using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CSVHelper
{
    public static List<T> FindAllObjectsFromResources<T>() {
		List<T> tmp = new List<T>();
		string ResourcePath = Application.dataPath + "/Resources";
		string[] directories = Directory.GetDirectories(ResourcePath, "*", SearchOption.AllDirectories);
		foreach (string directory in directories) {
			string directoryPath = directory.Substring(ResourcePath.Length + 1);
			T[] result = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>().ToArray();

			foreach (T  item in result) {
				if (!tmp.Contains(item)) {
					tmp.Add(item);
				}
			}
		}
		return tmp;
	}

	public static List<NodeGraphDataContainer> FindAllDialogueContainers() {
		string[] guids = AssetDatabase.FindAssets("t:DiaologueContainer");
		NodeGraphDataContainer[] items = new NodeGraphDataContainer[guids.Length];
		for (int i = 0; i < guids.Length; i++) {
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			items[i] = AssetDatabase.LoadAssetAtPath<NodeGraphDataContainer>(path);
		}
		return items.ToList();
	}
}
