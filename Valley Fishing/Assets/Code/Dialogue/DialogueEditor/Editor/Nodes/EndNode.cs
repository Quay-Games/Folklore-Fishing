using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class EndNode : BaseNode {

		private EndData endData = new EndData();
		public EndData EndData { 
			get => endData;
			set => endData = value; 
		}

		public EndNode() {

		}
		public EndNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, BaseData _data = null) 
			: base(_position, _editorWindow, _graphView, "USS/Nodes/EndNodeStyleSheet", "End", _data) {

			if(_data != null)
			{
                EndData.EndNodeType.Value = (_data as EndData).EndNodeType.Value;
				LoadValueInToField();
            }

			AddInputPort("Input", Port.Capacity.Multi);

			MakeMainContainer();
		}

		private void MakeMainContainer() {
			EnumField enumField = GetNewEnumField_EndNodeType(endData.EndNodeType);

			mainContainer.Add(enumField);
		}

		public override void LoadValueInToField() {
			if (EndData.EndNodeType.EnumField != null)
				EndData.EndNodeType.EnumField.SetValueWithoutNotify(EndData.EndNodeType.Value);
		}
	}
}
