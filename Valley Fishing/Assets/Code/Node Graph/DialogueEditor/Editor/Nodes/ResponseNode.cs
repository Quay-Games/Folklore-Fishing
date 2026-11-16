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

		public ResponseData ResponseData => (ResponseData)NodeData;

		public ResponseNode() {

		}
		public ResponseNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/BranchNodeStyleSheet", "Response", _data) {
			foreach (ResponseData_Text textData in ResponseData.ResponseData_Texts)
				BuildTextLine(textData);

			while (ResponseData.ResponseData_Texts.Count < 2)
				TextLine();

			ReloadLanguage();
			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("Option 1", Port.Capacity.Single);
			AddOutputPort("Option 2", Port.Capacity.Single);
		}

		public void TextLine(ResponseData_Text data_Text = null) {
			ResponseData_Text textData = data_Text ?? new ResponseData_Text();
			if (data_Text == null)
				textData.GuidID.Value = Guid.NewGuid().ToString();

			ResponseData.ResponseData_Texts.Add(textData);
			BuildTextLine(textData);
			ReloadLanguage();
		}

		private void BuildTextLine(ResponseData_Text textData) {
			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");
			AddTextField(textData, boxContainer);
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
