using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaitDatas.FishSpawnChance))]
public class FishSpawnChanceDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EditorGUI.BeginProperty(position, label, property);

		SerializedProperty fishChances =
			property.FindPropertyRelative("PerLocationFishSpawnChance");

		if (fishChances != null) {
			EditorGUI.PropertyField(position, fishChances, label, true);
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

		SerializedProperty fishChances =
			property.FindPropertyRelative("PerLocationFishSpawnChance");

		return fishChances != null
			? EditorGUI.GetPropertyHeight(fishChances, label, true)
			: EditorGUIUtility.singleLineHeight;
	}
}