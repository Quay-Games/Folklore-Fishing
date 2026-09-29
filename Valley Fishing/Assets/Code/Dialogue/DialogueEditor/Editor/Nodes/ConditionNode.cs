using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class ConditionNode : BaseNode {

		ConditionData nPCConditionData = new ConditionData();

		public ConditionData NPCConditionData {
			get => nPCConditionData;
			set => nPCConditionData = value;
		}

		public ConditionNode() {

		}

		public ConditionNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/EventNodeStyleSheet", "NPC Condition", _data) {
			if(_data != null)
            {
                NPCConditionData = _data as ConditionData;
                TextLine(NPCConditionData.EventData_EventName);
            }
            AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("True", Port.Capacity.Single);
			AddOutputPort("False", Port.Capacity.Single);
		}

		public void TextLine(EventData_EventName data_Text = null) {
			if (data_Text != null)
				NPCConditionData.EventData_EventName = data_Text;
			else if (NPCConditionData.EventData_EventName == null)
				NPCConditionData.EventData_EventName = new EventData_EventName();

			// Add container box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("EventName");

			// Add and set up the text field
			AddTextField(NPCConditionData.EventData_EventName, boxContainer);

			mainContainer.Add(boxContainer);
		}

		private void AddTextField(EventData_EventName container, Box boxContainer) {
			TextField textField = GetNewTextField(container.StringEventValue, "Text area", "TextBox");
			boxContainer.Add(textField);
		}

		public override BaseData Save()
		{
            List<Edge> edges = graphView.edges.ToList();

            Edge trueOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "True");
            Edge falseOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "False");

			ConditionData conditionData = new ConditionData();
			SaveData(conditionData);
			conditionData.trueGuidNode = trueOutput != null ? (trueOutput.input.node as BaseNode)?.NodeGuid : string.Empty;
			conditionData.falseGuidNode = falseOutput != null ? (falseOutput.input.node as BaseNode)?.NodeGuid : string.Empty;

            conditionData.EventData_EventName.StringEventValue.Value =
            NPCConditionData.EventData_EventName.StringEventValue.Value;

            return conditionData;
        }
	}
}
