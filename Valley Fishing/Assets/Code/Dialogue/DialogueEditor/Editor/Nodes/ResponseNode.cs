using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
    [NodeDataType(typeof(ResponseData))]
    public class ResponseNode : BaseNode {

		private ResponseData responseData = new ResponseData();
		public ResponseData ResponseData {
			get => responseData;
			set => responseData = value;
		}

		public ResponseNode() {

		}
		// TODO: make the optional data assign to the rest of the class
		public ResponseNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/BranchNodeStyleSheet", "Response", _data) {
			if (_data != null) {
				ResponseData = (_data as ResponseData);
                List<ResponseData_Text> textData = new List<ResponseData_Text>();
                if (ResponseData.ResponseData_Texts.Count > 0)
                {
                    for (int i = 0; i < ResponseData.ResponseData_Texts.Count; i++)
                    {
                        ResponseData_Text tmp = new ResponseData_Text();
                        tmp.GuidID = ResponseData.ResponseData_Texts[i].GuidID;
                        tmp.Text = ResponseData.ResponseData_Texts[i].Text;
                        tmp.ID = ResponseData.ResponseData_Texts[i].ID;
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
			ResponseData.ResponseData_Texts.Add(newDialogueBaseContainer_Text);

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

		public override BaseData Save()
		{
            List<Edge> edges = graphView.edges.ToList();

            List<Edge> tmpEdges = edges.Where(x => x.output.node == this).Cast<Edge>().ToList();

            Edge FirstOptionOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "Option 1");
            Edge SecondOptionOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "Option 2");


			ResponseData responseData = new ResponseData();
			SaveData(responseData);

			responseData.ResponseData_Texts = new List<ResponseData_Text>();
			responseData.SetBranchOneNodeGuid(FirstOptionOutput != null ? (FirstOptionOutput.input.node as BaseNode).NodeGuid : string.Empty);
			responseData.SetBranchTwoNodeGuid(SecondOptionOutput != null ? (SecondOptionOutput.input.node as BaseNode).NodeGuid : string.Empty);

            // Assign unique IDs and store response texts
            for (int i = 0; i < ResponseData.ResponseData_Texts.Count; i++)
            {
                ResponseData_Text original = ResponseData.ResponseData_Texts[i];

                ResponseData_Text textCopy = new ResponseData_Text
                {
                    ID = new Container_Int { Value = i },
                    GuidID = original.GuidID,
                    Text = new List<LanguageGeneric<string>>(original.Text)
                };

                responseData.ResponseData_Texts.Add(textCopy);
            }

            return responseData;
        }
	}
}
