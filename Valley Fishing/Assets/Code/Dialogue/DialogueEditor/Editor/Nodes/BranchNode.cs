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
    [NodeDataType(typeof(BranchData))]
    public class BranchNode : BaseNode {

		private BranchData branchData = new BranchData();
		public BranchData BranchData {
			get => branchData;
			set => branchData = value; 
		}

		public BranchNode() {

		}

		public BranchNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/BranchNodeStyleSheet", "Branch", _data)
        {
			if (_data != null)
			{
				BranchData = _data as BranchData;
                foreach (EventData_StringCondition item in BranchData.EventData_StringConditions)
                {
                    AddCondition(item);
                }

                ReloadLanguage();
            }

			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("True", Port.Capacity.Single);
			AddOutputPort("False", Port.Capacity.Single);

			TopButton();
		}

		private void TopButton() {
			ToolbarMenu Menu = new ToolbarMenu();
			Menu.text = "Add Condition";

			Menu.menu.AppendAction("String Event Condition", new Action<DropdownMenuAction>(x => AddCondition()));

			titleButtonContainer.Add(Menu);
		}

		public void AddCondition(EventData_StringCondition stringEvent = null) {
			AddStringConditionEventBuild(branchData.EventData_StringConditions, stringEvent);
		}

		public override BaseData Save()
		{
            List<Edge> edges = graphView.edges.ToList();
            List<Edge> tmpEdges = edges.Where(x => x.output.node == this).Cast<Edge>().ToList();

            Edge trueOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "True");
            Edge flaseOutput = edges.FirstOrDefault(x => x.output.node == this && x.output.portName == "False");

            BranchData branchData = new BranchData();
			SaveData(branchData);
            branchData.SetBranchOneNodeGuid((trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGuid : string.Empty));
			branchData.SetBranchTwoNodeGuid((flaseOutput != null ? (flaseOutput.input.node as BaseNode).NodeGuid : string.Empty));

            foreach (EventData_StringCondition stringEvents in BranchData.EventData_StringConditions)
            {
                EventData_StringCondition tmp = new EventData_StringCondition();
                tmp.StringEventValue.Value = stringEvents.StringEventValue.Value;
                tmp.StringEventText.Value = stringEvents.StringEventText.Value;
                tmp.StringEventConditionType.Value = stringEvents.StringEventConditionType.Value;

                branchData.EventData_StringConditions.Add(tmp);
            }

            return branchData;

        }
    }
}
