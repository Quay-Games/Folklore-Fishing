using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class ResponseNode : BaseNode {

		private ResponseData responseData = new ResponseData();
		public ResponseData ResponseData {
			get => responseData;
			set => responseData = value;
		}

		public ResponseNode() {

		}
		// TODO: make the optional data assign to the rest of the class
		public ResponseNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/BranchNodeStyleSheet", "Response", _data) {
			if (_data != null) {
				ResponseData = (_data as ResponseData);
                List<ResponseData_Text> textData = new List<ResponseData_Text>();
                if (ResponseData.ResponceData_Texts.Count > 0)
                {
                    for (int i = 0; i < ResponseData.ResponceData_Texts.Count; i++)
                    {
                        ResponseData_Text tmp = new ResponseData_Text();
                        tmp.GuidID = ResponseData.ResponceData_Texts[i].GuidID;
                        tmp.Text = ResponseData.ResponceData_Texts[i].Text;
                        tmp.ID = ResponseData.ResponceData_Texts[i].ID;
                        textData.Add(tmp);
                    }
                    TextLine(textData[0]);
                    TextLine(textData[1]);
                }
                else
                {
                    TextLine();
                    TextLine();
                }

                ReloadLanguage();
            }
			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("Option 1", Port.Capacity.Single);
			AddOutputPort("Option 2", Port.Capacity.Single);
		}

		public void TextLine(ResponseData_Text data_Text = null) {
			ResponseData_Text newDialogueBaseContainer_Text = new ResponseData_Text();
			ResponseData.ResponceData_Texts.Add(newDialogueBaseContainer_Text);

			// Add Container Box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");

			// Add Fields
			AddTextField(newDialogueBaseContainer_Text, boxContainer);

			// Load in data if it got any
			if (data_Text != null) {
				// Guid ID
				newDialogueBaseContainer_Text.GuidID = data_Text.GuidID;

				// Text
				foreach (LanguageGeneric<string> data_text in data_Text.Text) {
					foreach (LanguageGeneric<string> text in newDialogueBaseContainer_Text.Text) {
						if (text.LanguageType == data_text.LanguageType) {
							text.LanguageGenericType = data_text.LanguageGenericType;
						}
					}
				}

			} else {
				// Make New Guid ID
				newDialogueBaseContainer_Text.GuidID.Value = Guid.NewGuid().ToString();
			}

			// Reaload the current selected language
			ReloadLanguage();

			mainContainer.Add(boxContainer);
		}

		private void AddTextField(ResponseData_Text container, Box boxContainer) {
			TextField textField = GetNewTextField_TextLanguage(container.Text, "Text area", "TextBox");
			container.TextField = textField;
			boxContainer.Add(textField);
		}

		public override void ReloadLanguage() {
			base.ReloadLanguage();
		}
	}
}
