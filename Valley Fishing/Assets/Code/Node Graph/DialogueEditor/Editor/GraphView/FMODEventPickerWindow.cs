using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using FMODUnity;


public class FMODEventPickerWindow : EditorWindow {
	private Action<EventReference> onSelected;

	private FMODEventDatabase database;


	public static void Open(Action<EventReference> callback) {
		FMODEventPickerWindow window =
			CreateInstance<FMODEventPickerWindow>();

		window.titleContent =
			new GUIContent("FMOD Event Picker");

		window.onSelected = callback;

		window.ShowUtility();
	}


	private void CreateGUI() {
		database = LoadDatabase();

		if (database == null) {
			rootVisualElement.Add(
				new Label("FMOD Event Database not found")
			);

			return;
		}


		VisualElement searchContainer = new VisualElement();

		TextField searchField = new TextField {
			label = "Search"
		};


		ScrollView eventList = new ScrollView();


		searchContainer.Add(searchField);

		rootVisualElement.Add(searchContainer);
		rootVisualElement.Add(eventList);


		void RefreshList() {
			eventList.Clear();

			string search =
				searchField.value.ToLower();


			foreach (EventReference evt in database.Events) {
				if (!string.IsNullOrEmpty(search) &&
					!evt.Path.ToLower().Contains(search)) {
					continue;
				}


				Button button = new Button();


				button.text = evt.Path;


				button.clicked += () => {
					onSelected?.Invoke(evt);

					Close();
				};


				eventList.Add(button);
			}
		}


		searchField.RegisterValueChangedCallback(evt => {
			RefreshList();
		});


		RefreshList();
	}


	private FMODEventDatabase LoadDatabase() {
		return AssetDatabase.LoadAssetAtPath<FMODEventDatabase>(
			"Assets/FMODEventDatabase.asset"
		);
	}
}