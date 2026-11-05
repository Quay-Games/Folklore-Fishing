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
    public class VocalOverrideNode : BaseNode {
		private NPCDialogueData dialogueData = new NPCDialogueData();

		public NPCDialogueData DialogueData {
			get => dialogueData;
			set => dialogueData = value;
		}
		public	VocalOverrideNode() {

		}

		public VocalOverrideNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "Voice Line Override", _data)
        {
			AddGenericPorts();

            if (_data != null) {
				DialogueData = _data as NPCDialogueData;

                DialogueTextData textData = new DialogueTextData
                {
                    GuidID = DialogueData.DialogueText.GuidID,
                    Text = DialogueData.DialogueText.Text,
                    ID = DialogueData.DialogueText.ID
                };


                ReloadLanguage();
            }
		}


        public override BaseData Save()
        {
			//originally there was a simple call to the original method, but I realized you cannot cast objects forward
			//into complex types, you can only cast backwards to more simple types
			NPCDialogueData data = new NPCDialogueData();
			SaveData(data);
			data.VoiceEvent = dialogueData.VoiceEvent;
			data.ProgressGraph = dialogueData.ProgressGraph;

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
