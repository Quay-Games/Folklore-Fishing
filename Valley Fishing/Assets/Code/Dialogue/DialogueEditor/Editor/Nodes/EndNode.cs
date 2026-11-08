using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
    [NodeDataType(typeof(EndData))]
    public class EndNode : BaseNode {

        public EndData EndData => (EndData)NodeData;

        public EndNode() {

		}
		public EndNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null) 
			: base(_position, _editorWindow, _graphView, "USS/Nodes/EndNodeStyleSheet", "End", _data) {

			AddInputPort("Input", Port.Capacity.Multi);
		}

		public override BaseData Save()
		{
			EndData data = new EndData();
			SaveData(data);
            data.EndNodeType.Value = EndData.EndNodeType.Value;
            return data;
        }
	}
}
