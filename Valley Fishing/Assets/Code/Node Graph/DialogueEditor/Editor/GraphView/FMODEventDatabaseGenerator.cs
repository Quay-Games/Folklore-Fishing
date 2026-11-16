using UnityEditor;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;


public static class FMODEventDatabaseGenerator {
	private const string SavePath =
		"Assets/FMODEventDatabase.asset";


	[MenuItem("Tools/FMOD/Generate Event Database")]
	public static void Generate() {
		FMODEventDatabase database =
			AssetDatabase.LoadAssetAtPath<FMODEventDatabase>(SavePath);


		if (database == null) {
			database = ScriptableObject.CreateInstance<FMODEventDatabase>();
			AssetDatabase.CreateAsset(database, SavePath);
		}


		database.Events.Clear();


		FMOD.Studio.System studioSystem =
			RuntimeManager.StudioSystem;


		studioSystem.getBankList(out Bank[] banks);


		foreach (Bank bank in banks) {
			bank.getEventList(out EventDescription[] events);


			foreach (EventDescription eventDescription in events) {
				eventDescription.getPath(out string path);
				eventDescription.getID(out FMOD.GUID guid);


				EventReference reference = new EventReference {
					Guid = guid,
					Path = path
				};


				database.Events.Add(reference);
			}
		}


		EditorUtility.SetDirty(database);
		AssetDatabase.SaveAssets();


		Debug.Log(
			$"Generated FMOD Event Database: {database.Events.Count} events"
		);
	}
}
