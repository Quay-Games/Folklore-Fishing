using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.DialogueEditor {
	public class CustomTools : MonoBehaviour {
		[MenuItem("Custom Tools/Dialogue/Save to CSV")]
		public static void SaveToCSV() {
			SaveCSV saveCSV = new SaveCSV();
			saveCSV.Save();
			EditorApplication.Beep();
		}

		[MenuItem("Custom Tools/Dialogue/Load from CSV")]

		public static void LoadFromCSV() {
			LoadCSV loadCSV = new LoadCSV();
			loadCSV.Load();
			EditorApplication.Beep();
		}

		[MenuItem("Custom Tools/Dialogue/Update Dialogue Languages")]
		public static void UpdateDialogueLanguage() {
			UpdateLanguageType updateLanguageType = new UpdateLanguageType();
			updateLanguageType.UpdateLanguage();

			EditorApplication.Beep();
		}
	}
}
