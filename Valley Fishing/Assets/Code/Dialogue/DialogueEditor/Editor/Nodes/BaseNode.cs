using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class BaseNode : Node {
		public string nodeGuid;
		protected DialogueGraphView graphView;
		protected DialogueEditorWindow editorWindow;
		protected Vector2 defaultNodeSize = new Vector2(200, 250);

		private List<LanguageGenericHolder_Text> languageGenericHolder_Texts = new List<LanguageGenericHolder_Text>();

		public string NodeGuid {
			get => nodeGuid;
			set => nodeGuid = value;
		}

		public BaseNode() {
			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/NodeStyleSheet");
			styleSheets.Add(styleSheet);
		}

		//here is a special constructor that takes care of redundant operations all nodes will need to do with this kind of data
		public BaseNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, string _stylesheet, string _title, BaseData data = null)
        {
            editorWindow = _editorWindow;
            graphView = _graphView;
			title = _title;
			if(data != null)
            {
                nodeGuid = data.NodeGuid;
                SetPosition(new Rect(data.Position, defaultNodeSize));
            } else
			{
                nodeGuid = Guid.NewGuid().ToString();
                SetPosition(new Rect(_position, defaultNodeSize));
            }

            StyleSheet styleSheet = Resources.Load<StyleSheet>(_stylesheet);
			styleSheets.Add(styleSheet);
        }

		protected Label GetNewLabel(string labelName, string USS01 = "", string USS02 = "") {
			Label label_texts = new Label(labelName);
			label_texts.AddToClassList(USS01);
			label_texts.AddToClassList(USS02);

			return label_texts;
		}

		protected Button GetNewButton(string btnText, string USS01 = "", string USS02 = "") {
			Button btn = new Button() {
				text = btnText,
			};

			btn.AddToClassList(USS01);
			btn.AddToClassList(USS02);

			return btn;
		}
		protected IntegerField GetNewIntegerField(Container_Int inputValue, string USS01 = "", string USS02 = "") {
			IntegerField integerField = new IntegerField();

			integerField.RegisterValueChangedCallback(value => {
				inputValue.Value = value.newValue;
			});
			integerField.SetValueWithoutNotify(inputValue.Value);

			integerField.AddToClassList(USS01);
			integerField.AddToClassList(USS02);

			return integerField;
		}

		protected FloatField GetNewFloatField(Container_Float inputValue, string USS01 = "", string USS02 = "") {
			FloatField floatField = new FloatField();

			floatField.RegisterValueChangedCallback(value => {
				inputValue.Value = value.newValue;
			});
			floatField.SetValueWithoutNotify(inputValue.Value);

			floatField.AddToClassList(USS01);
			floatField.AddToClassList(USS02);

			return floatField;
		}
		protected PropertyField GetNewEventReferenceField( SerializedObject serializedObject, string propertyName, string USS01 = "",	string USS02 = "") {
			SerializedProperty property = serializedObject.FindProperty(propertyName);

			PropertyField field = new PropertyField(property);

			field.AddToClassList(USS01);
			field.AddToClassList(USS02);

			field.Bind(serializedObject);

			return field;
		}

		protected TextField GetNewTextField(Container_String inputValue, string placeholderText, string USS01 = "", string USS02 = "") {
			TextField textField = new TextField();

			textField.RegisterValueChangedCallback(value => {
				inputValue.Value = value.newValue;
			});
			textField.SetValueWithoutNotify(inputValue.Value);

			textField.AddToClassList(USS01);
			textField.AddToClassList(USS02);

			SetPlaceholderText(textField, placeholderText);

			return textField;
		}

		protected EnumField GetNewEnumField_ChoiceStateType(Container_ChoiceStateType enumType, string USS01 = "", string USS02 = "") {
			EnumField enumField = new EnumField() {
				value = enumType.Value
			};
			enumField.Init(enumType.Value);

			enumField.RegisterValueChangedCallback((value) => {
				enumType.Value = (ChoiceStateType)value.newValue;
			});
			enumField.SetValueWithoutNotify(enumType.Value);

			enumField.AddToClassList(USS01);
			enumField.AddToClassList(USS02);

			enumType.EnumField = enumField;
			return enumField;
		}

		protected EnumField GetNewEnumField_EndNodeType(Container_EndNodeType enumType, string USS01 = "", string USS02 = "") {
			EnumField enumField = new EnumField() {
				value = enumType.Value
			};
			enumField.Init(enumType.Value);

			enumField.RegisterValueChangedCallback((value) => {
				enumType.Value = (EndNodeType)value.newValue;
			});
			enumField.SetValueWithoutNotify(enumType.Value);

			enumField.AddToClassList(USS01);
			enumField.AddToClassList(USS02);

			enumType.EnumField = enumField;
			return enumField;
		}

		protected EnumField GetNewEnumField_StringEventModifierType(Container_StringEventModifierType enumType, Action action, string USS01 = "", string USS02 = "") {
			EnumField enumField = new EnumField() {
				value = enumType.Value
			};
			enumField.Init(enumType.Value);

			enumField.RegisterValueChangedCallback((value) => {
				enumType.Value = (StringEventModifierType)value.newValue;
				action?.Invoke();
			});
			enumField.SetValueWithoutNotify(enumType.Value);

			enumField.AddToClassList(USS01);
			enumField.AddToClassList(USS02);

			enumType.EnumField = enumField;
			return enumField;
		}

		protected EnumField GetNewEnumField_StringEventConditionType(Container_StringEventConditionType enumType, Action action, string USS01 = "", string USS02 = "") {
			EnumField enumField = new EnumField() {
				value = enumType.Value
			};
			enumField.Init(enumType.Value);

			enumField.RegisterValueChangedCallback((value) => {
				enumType.Value = (StringEventConditionType)value.newValue;
				action?.Invoke();
			});
			enumField.SetValueWithoutNotify(enumType.Value);

			enumField.AddToClassList(USS01);
			enumField.AddToClassList(USS02);

			enumType.EnumField = enumField;
			return enumField;
		}

		protected TextField GetNewTextField_TextLanguage(List<LanguageGeneric<string>> Text, string placeholderText = "", string USS01 = "", string USS02 = "") {
			foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType))) {
				Text.Add(new LanguageGeneric<string> {
					LanguageType = language,
					LanguageGenericType = ""
				});
			}

			TextField textField = new TextField("");

			languageGenericHolder_Texts.Add(new LanguageGenericHolder_Text(Text, textField, placeholderText));

			textField.RegisterValueChangedCallback(value => {
				Text.Find(text => text.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
			});

			textField.SetValueWithoutNotify(Text.Find(text => text.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType);

			textField.multiline = true;

			textField.AddToClassList(USS01);
			textField.AddToClassList(USS02);

			return textField;
		}
		protected ObjectField GetNewObjectField_ListenEvent(Container_ListenEventSO inputListenEventSO, string USS01 = "", string USS02 = "") {
			ObjectField objectField = new ObjectField() {
				objectType = typeof(ListenEventSO),
				allowSceneObjects = false,
				value = inputListenEventSO.ListenEventSO,
			};

			// When we change the variable from graph view.
			objectField.RegisterValueChangedCallback(value => {
				inputListenEventSO.ListenEventSO = value.newValue as ListenEventSO;
			});
			objectField.SetValueWithoutNotify(inputListenEventSO.ListenEventSO);

			// Set uss class for stylesheet.
			objectField.AddToClassList(USS01);
			objectField.AddToClassList(USS02);

			return objectField;
		}

		protected void AddStringModifierEventBuild(List<EventData_StringModifier> stringEventModifier, EventData_StringModifier stringEvent = null) {
			EventData_StringModifier tmpStringEventModifier = new EventData_StringModifier();

			if (stringEvent != null) {
				tmpStringEventModifier.StringEventText.Value = stringEvent.StringEventText.Value;
				tmpStringEventModifier.StringEventValue.Value = stringEvent.StringEventValue.Value;
				tmpStringEventModifier.StringEventModifierType.Value = stringEvent.StringEventModifierType.Value;
			}

			stringEventModifier.Add(tmpStringEventModifier);

			Box boxContainer = new Box();
			Box boxfloatField = new Box();
			boxContainer.AddToClassList("StringEventBox");
			boxfloatField.AddToClassList("StringEventBoxfloatField");

			TextField textField = GetNewTextField(tmpStringEventModifier.StringEventText, "String Event", "StringEventText");

			TextField floatField = GetNewTextField(tmpStringEventModifier.StringEventValue, "StringEventInt");

			Action tmp = () => ShowHide_StringEventModifierType(tmpStringEventModifier.StringEventModifierType.Value, boxfloatField);
			EnumField enumField = GetNewEnumField_StringEventModifierType(tmpStringEventModifier.StringEventModifierType, tmp, "StringEventEnum");
			ShowHide_StringEventModifierType(tmpStringEventModifier.StringEventModifierType.Value, boxfloatField);

			Button btn = GetNewButton("X", "removeBtn");
			btn.clicked += () => {
				stringEventModifier.Remove(tmpStringEventModifier);
				DeleteBox(boxContainer);
			};

			boxContainer.Add(textField);
			boxContainer.Add(enumField);
			boxfloatField.Add(floatField);
			boxContainer.Add(boxfloatField);
			boxContainer.Add(btn);

			mainContainer.Add(boxContainer);
			RefreshExpandedState();
		}

		protected void AddStringConditionEventBuild(List<EventData_StringCondition> stringEventCondition, EventData_StringCondition stringEvent = null) {
			EventData_StringCondition tmpStringEventCondition = new EventData_StringCondition();

			if (stringEvent != null) {
				tmpStringEventCondition.StringEventText.Value = stringEvent.StringEventText.Value;
				tmpStringEventCondition.StringEventValue.Value = stringEvent.StringEventValue.Value;
				tmpStringEventCondition.StringEventConditionType.Value = stringEvent.StringEventConditionType.Value;
			}

			stringEventCondition.Add(tmpStringEventCondition);

			Box boxContainer = new Box();
			Box boxfloatField = new Box();
			boxContainer.AddToClassList("StringEventBox");
			boxfloatField.AddToClassList("StringEventBoxfloatField");

			TextField textField = GetNewTextField(tmpStringEventCondition.StringEventText, "String Event", "StringEventText");

			TextField floatField = GetNewTextField(tmpStringEventCondition.StringEventValue, "StringEventInt");

			EnumField enumField = null;
			Action tmp = () => ShowHide_StringEventConditionType(tmpStringEventCondition.StringEventConditionType.Value, boxfloatField);
			enumField = GetNewEnumField_StringEventConditionType(tmpStringEventCondition.StringEventConditionType, tmp, "StringEventEnum");
			ShowHide_StringEventConditionType(tmpStringEventCondition.StringEventConditionType.Value, boxfloatField);

			Button btn = GetNewButton("X", "removeBtn");
			btn.clicked += () => {
				stringEventCondition.Remove(tmpStringEventCondition);
				DeleteBox(boxContainer);
			};

			boxContainer.Add(textField);
			boxContainer.Add(enumField);
			boxfloatField.Add(floatField);
			boxContainer.Add(boxfloatField);
			boxContainer.Add(btn);

			mainContainer.Add(boxContainer);
			RefreshExpandedState();
		}

		private void ShowHide_StringEventModifierType(StringEventModifierType value, Box boxContainer) {
			if (value == StringEventModifierType.SetTrue || value == StringEventModifierType.SetFalse) {
				ShowHide(false, boxContainer);
			} else {
				ShowHide(true, boxContainer);
			}
		}

		private void ShowHide_StringEventConditionType(StringEventConditionType value, Box boxContainer) {
			if (value == StringEventConditionType.True || value == StringEventConditionType.False) {
				ShowHide(false, boxContainer);
			} else {
				ShowHide(true, boxContainer);
			}
		}

		protected void ShowHide(bool show, Box boxContainer) {
			string hideUssClass = "Hide";
			if (show == true) {
				boxContainer.RemoveFromClassList(hideUssClass);
			} else {
				boxContainer.AddToClassList(hideUssClass);
			}
		}

		protected virtual void DeleteBox(Box boxContainer) {
			mainContainer.Remove(boxContainer);
			RefreshExpandedState();
		}

		public Port AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single) {
		Port outputPort = GetPortInstance(Direction.Output, capacity);
		outputPort.portName = name;
		outputContainer.Add(outputPort);
		return outputPort;
		}

		public Port AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi) {
			Port inputPort = GetPortInstance(Direction.Input, capacity);
			inputPort.portName = name;
			inputContainer.Add(inputPort);
			return inputPort; ;
		}

		public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single) {
			return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
		}

		public virtual void LoadValueInToField() {

		}

		public virtual void ReloadLanguage() {
			foreach (LanguageGenericHolder_Text textHolder in languageGenericHolder_Texts) {
				Reload_TextLanguage(textHolder.inputText, textHolder.textField, textHolder.placeholderText);
			}
		}

		protected void Reload_TextLanguage(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "") {
			textField.RegisterValueChangedCallback(value => {
				inputText.Find(text => text.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
			});
			textField.SetValueWithoutNotify(inputText.Find(text => text.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType);

			SetPlaceholderText(textField, placeholderText);
		}

		protected void SetPlaceholderText(TextField textField, string placeholder) {
			string placeholderClass = TextField.ussClassName + "__placeholder";

			CheckForText();
			onFocusOut();
			textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
			textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

			void onFocusIn() {
				if (textField.ClassListContains(placeholderClass)) {
					textField.value = string.Empty;
					textField.RemoveFromClassList(placeholderClass);
				}
			}

			void onFocusOut() {
				if (string.IsNullOrEmpty(textField.text)) {
					textField.SetValueWithoutNotify(placeholder);
					textField.AddToClassList(placeholderClass);
				}
			}

			void CheckForText() {
				if (!string.IsNullOrEmpty(textField.text)) {
					textField.RemoveFromClassList(placeholderClass);
				}
			}
		}

		class LanguageGenericHolder_Text {
			public LanguageGenericHolder_Text(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "placeholder text") {
				this.inputText = inputText;
				this.textField = textField;
				this.placeholderText = placeholderText;
			}
			public List<LanguageGeneric<string>> inputText;
			public TextField textField;
			public string placeholderText;
		}

        #region Griff Extra Functions
		public virtual BaseData Save()
		{
			return null;
		}

		//public virtual void Load(BaseData data, DialogueContainer container, DialogueGraphView graphView)
		//{
		//}
        #endregion

    }
}
