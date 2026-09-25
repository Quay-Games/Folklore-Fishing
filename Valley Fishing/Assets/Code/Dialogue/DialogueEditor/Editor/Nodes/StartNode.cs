using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class StartNode : BaseNode {
		public StartNode() {

		}

		//does a start node make use of basedata properly?
		public StartNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/StartNodeStyleSheet", "Start", _data)
        {
			AddOutputPort("Output", Port.Capacity.Single);

			RefreshExpandedState();
			RefreshPorts();
		}

	}
}
