using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
    [NodeDataType(typeof(NPCDialogueData))]
    public class DialogueNode : BaseNode {
		public NPCDialogueData DialogueData => (NPCDialogueData)NodeData;
		public DialogueNode() {

		}

		public DialogueNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "NPC Dialogue", _data)
        {
			AddGenericPorts();

			TextLine(DialogueData.DialogueText);
		}


		public void TextLine(DialogueTextData data_Text = null) {
			DialogueTextData textData = data_Text ?? new DialogueTextData();
			if (data_Text == null)
				textData.GuidID.Value = Guid.NewGuid().ToString();

			DialogueData.DialogueText = textData;
			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");


            TextField textField = GetNewTextField_TextLanguage(
				textData.Text,
				"Text area",
				"TextBox"
			);

            boxContainer.Add(textField);


			ReloadLanguage();

			mainContainer.Add(boxContainer);
		}


		public override void ReloadLanguage() {
			base.ReloadLanguage();
		}


		public override void LoadValueInToField() {

		}

        public override BaseData Save()
        {
			//originally there was a simple call to the original method, but I realized you cannot cast objects forward
			//into complex types, you can only cast backwards to more simple types
			NPCDialogueData data = new NPCDialogueData();
			SaveData(data);
			data.VoiceEvent = DialogueData.VoiceEvent;
			data.ProgressGraph = DialogueData.ProgressGraph;

            DialogueTextData tmp = DialogueData.DialogueText;

            if (tmp != null)
            {
                data.DialogueText = new DialogueTextData
                {
                    ID = tmp.ID,
                    GuidID = tmp.GuidID,
                    Text = new List<LanguageGeneric<string>>(tmp.Text)
                };
            }

            return data;
        }
	}
}
