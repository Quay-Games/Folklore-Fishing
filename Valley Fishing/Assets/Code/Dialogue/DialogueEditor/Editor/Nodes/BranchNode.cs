using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class BranchNode : BaseNode {

		private BranchData branchData = new BranchData();
		public BranchData BranchData {
			get => branchData;
			set => branchData = value; 
		}

		public BranchNode() {

		}

		public BranchNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, BaseData _data = null)
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
	}
}
