using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class StartNode : BaseNode {
		public StartNode() {

		}

		//does a start node make use of basedata properly?
		public StartNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
		: base(_position, _editorWindow, _graphView, "USS/Nodes/StartNodeStyleSheet", "Start", _data)
        {
			AddOutputPort("Output", Port.Capacity.Single);

			RefreshExpandedState();
			RefreshPorts();
		}

		//this is only necessary because otherwise a start node will get saved as a basenode
		public override BaseData Save()
        {
            StartData data = new StartData();
            SaveData(data);
            return data;
        }
    }
}
