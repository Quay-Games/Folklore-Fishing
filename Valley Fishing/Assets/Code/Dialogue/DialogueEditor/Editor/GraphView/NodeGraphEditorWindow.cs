using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class NodeGraphEditorWindow : EditorWindow {
		private DialogueContainer currentDialogueContainer;
		private NodeGraphView graphView;
		private NodeGraphSaveAndLoad saveAndLoad;

		private LanguageType selectLanguage = LanguageType.English;
		private ToolbarMenu languageDropdownMenu;
		private Label nameOfDialogueContainer;
		private string graphViewStyleSheetName = "USS/EditorWindow/EditorWindowStyleSheet";

		public LanguageType SelectedLanguage {
			get => selectLanguage;
			set => selectLanguage = value;
		}

		[OnOpenAsset(0)]
		public static bool ShowWindow(int instanceId, int line) {
			UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceId);
			if (item is DialogueContainer) {
				NodeGraphEditorWindow window = (NodeGraphEditorWindow)GetWindow(typeof(NodeGraphEditorWindow));
				window.titleContent = new GUIContent("Dialogue Editor");
				window.currentDialogueContainer = item as DialogueContainer;
				window.minSize = new Vector2(500, 250);
				window.Load();
			}
			return false;
		}

		private void OnEnable() {
			ConstructGraphView();
			GenerateToolbar();
			Load();
		}

		private void OnDisable() {
			rootVisualElement.Remove(graphView);
		}

		private void ConstructGraphView() {
			graphView = new NodeGraphView(this);
			graphView.StretchToParentSize();
			rootVisualElement.Add(graphView);
			saveAndLoad = new NodeGraphSaveAndLoad(graphView);
		}

		private void GenerateToolbar() {
			StyleSheet styleSheet = Resources.Load<StyleSheet>(graphViewStyleSheetName);
			rootVisualElement.styleSheets.Add(styleSheet);
			Toolbar toolbar = new Toolbar();
			//save button
			Button saveButton = new Button() {
				text = "Save",
			};
			saveButton.clicked += () => {
				Save();
			};
			toolbar.Add(saveButton);
			//load button
			Button loadButton = new Button() {
				text = "Load",
			};
			loadButton.clicked += () => {
				Load();
			};
			toolbar.Add(loadButton);
			//language dropdown
			languageDropdownMenu = new ToolbarMenu();

			foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType))) {
				languageDropdownMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language)));
			}
			toolbar.Add(languageDropdownMenu);
			//name of current dialoge container
			nameOfDialogueContainer = new Label("");
			toolbar.Add(nameOfDialogueContainer);
			nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");


			rootVisualElement.Add(toolbar);
		}

		private void Load() {
			if (currentDialogueContainer != null) {
				Language(LanguageType.English);
				nameOfDialogueContainer.text = "Name:   " + currentDialogueContainer.name;
				saveAndLoad.Load(currentDialogueContainer);
			}
		}

		private void Save() {
			if (currentDialogueContainer != null) {
				saveAndLoad.Save(currentDialogueContainer);
			}
		}

		private void Language(LanguageType _language) {
			languageDropdownMenu.text = "Language:  " + _language.ToString();
			selectLanguage = _language;
			graphView.ReloadLanguage();
		}
	}
}
