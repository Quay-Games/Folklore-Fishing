using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(InputStateData))]
    public class InputStateNode : BaseNode
    {
        public InputStateData InputData => (InputStateData)NodeData;

        public InputStateNode()
        {

        }
        public InputStateNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Input State Change", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            InputStateData data = new InputStateData();
            SaveData(data);
            data.InputStateValues.Value = InputData.InputStateValues.Value;
            return data;
        }
    }
}
