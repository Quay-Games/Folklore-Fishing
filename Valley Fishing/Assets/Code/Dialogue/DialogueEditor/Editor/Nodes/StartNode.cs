using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
    [NodeDataType(typeof(StartData))]
    public class StartNode : BaseNode {
		public StartNode() {

		}

		public StartNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/StartNodeStyleSheet", "Start", _data)
        {
			AddOutputPort("Output", Port.Capacity.Single);

			RefreshExpandedState();
			RefreshPorts();
		}

		public override BaseData Save()
        {
            SaveData(NodeData);
            return NodeData;
        }
    }
}
